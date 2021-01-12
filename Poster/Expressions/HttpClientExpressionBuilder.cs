namespace Poster.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading;
    using Abstract;
    using Abstraction;
    using Attributes;
    using Attributes.Abstract;
    using Core.Types;
    using Exceptions;
    using Extensions;
    using Http.Clients;
    using Http.Clients.Abstract;
    using Http.Models;
    using Http.Serializers.Abstract;
    using Http.Url;
    using Http.Url.Abstract;

    /// <summary>
    /// Represents expression builder that can build expression for <see cref="Http.Clients.Abstract.IHttpClient"/> method invocation.
    /// </summary>
    public class HttpClientExpressionBuilder : IHttpClientExpressionBuilder
    {
        private readonly IHttpClient _httpClient;
        private readonly IUrlBuilder _urlBuilder;
        private readonly MethodInfo _httpClientGetMethod;
        private readonly MethodInfo _httpClientPostMethod;
        private readonly MethodInfo _httpClientPutMethod;
        private readonly MethodInfo _httpClientDeleteMethod;
        private readonly MethodInfo _httpClientPatchMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientExpressionBuilder"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Http client factory that will use for <see cref="HttpClient"/> building.</param>
        /// <param name="contentSerializer"><see cref="IContentSerializer"/> that will be used for http content serialization.</param>
        /// <param name="httpClient">Http client.</param>
        public HttpClientExpressionBuilder(
            IHttpClientFactory httpClientFactory,
            IContentSerializer contentSerializer,
            IHttpClient? httpClient = null)
        {
            _httpClient = httpClient ?? new PosterHttpClient(httpClientFactory, contentSerializer, nameof(IPoster));
            _urlBuilder = new UrlBuilder();

            var httpClientType = _httpClient.GetType();

            _httpClientGetMethod = httpClientType.GetMethod(nameof(IHttpClient.GetAsync)) ??
                                   throw new Exception($"{nameof(IHttpClient)} doesn't contain {nameof(IHttpClient.GetAsync)} method");
            _httpClientPostMethod = httpClientType.GetMethod(nameof(IHttpClient.PostAsync)) ??
                                    throw new Exception($"{nameof(IHttpClient)} doesn't contain {nameof(IHttpClient.PostAsync)} method");
            _httpClientPutMethod = httpClientType.GetMethod(nameof(IHttpClient.PutAsync)) ??
                                   throw new Exception($"{nameof(IHttpClient)} doesn't contain {nameof(IHttpClient.PutAsync)} method");
            _httpClientDeleteMethod = httpClientType.GetMethod(nameof(IHttpClient.DeleteAsync)) ??
                                      throw new Exception($"{nameof(IHttpClient)} doesn't contain {nameof(IHttpClient.DeleteAsync)} method");
            _httpClientPatchMethod = httpClientType.GetMethod(nameof(IHttpClient.PatchAsync)) ??
                                     throw new Exception($"{nameof(IHttpClient)} doesn't contain {nameof(IHttpClient.PatchAsync)} method");
        }

        /// <inheritdoc/>
        public LambdaExpression GetExpressionForGenericTask(MethodInfo serviceMethod)
            => GetExpressionForTask(
                serviceMethod.GetParameters(),
                serviceMethod.ReturnType.GenericTypeArguments.First(),
                GetHttpAttribute(serviceMethod.GetCustomAttributes()));

        /// <inheritdoc/>
        public LambdaExpression GetExpressionForNonGenericTask(MethodInfo serviceMethod)
            => GetExpressionForTask(
                serviceMethod.GetParameters(),
                typeof(Empty),
                GetHttpAttribute(serviceMethod.GetCustomAttributes()));

        private static ParameterInfo? GetBodyParameter(IEnumerable<ParameterInfo> parameterInfos)
            => parameterInfos.FirstOrDefault(p => p.GetCustomAttribute<BodyAttribute>() != null);

        private static ParameterInfo? GetCancellationTokenParameter(IEnumerable<ParameterInfo> parameterInfos)
            => parameterInfos.FirstOrDefault(p => p.ParameterType == typeof(CancellationToken));

        private static HttpAttribute? GetHttpAttribute(IEnumerable<Attribute> attributes)
            => attributes
                .FirstOrDefault(a => a is GetAttribute
                                     || a is PostAttribute
                                     || a is PatchAttribute
                                     || a is PutAttribute
                                     || a is DeleteAttribute) as HttpAttribute;

        private static void ValidateServiceMethodParameters(ParameterInfo[] parameters, HttpAttribute httpAttribute)
        {
            if(parameters.Any(p => p.GetCustomAttribute<BodyAttribute>() != null && (httpAttribute is GetAttribute || httpAttribute is DeleteAttribute)))
                throw new ServiceInitializeException("GET or DELETE http request cannot has body");
        }

        private LambdaExpression GetExpressionForTask(ParameterInfo[] parameters, Type taskGenericArgumentType, HttpAttribute? httpAttribute)
        {
            if(httpAttribute is null)
                throw new ServiceInitializeException("Service methods should contain http attribute");

            ValidateServiceMethodParameters(parameters, httpAttribute);

            if (httpAttribute is PostAttribute || httpAttribute is PutAttribute || httpAttribute is PatchAttribute)
                return GetExpressionForBodyRequest(
                    httpAttribute,
                    taskGenericArgumentType,
                    parameters);

            return GetExpressionForBodilessRequest(
                httpAttribute,
                taskGenericArgumentType,
                parameters);
        }

        private LambdaExpression GetExpressionForBodilessRequest(HttpAttribute httpAttribute, Type returnType, ParameterInfo[] parameters)
        {
            var parameterExpressions = parameters
                .Select(p => (Expression.Parameter(p.ParameterType, p.Name), p))
                .ToArray();

            return GetExpressionForRequest(
                httpAttribute,
                returnType,
                parameters,
                parameterExpressions,
                null);
        }

        private LambdaExpression GetExpressionForBodyRequest(HttpAttribute httpAttribute, Type returnType, ParameterInfo[] parameters)
        {
            var parameterExpressions = parameters
                .Select(p => (Expression.Parameter(p.ParameterType, p.Name), p))
                .ToArray();
            var bodyParameter = GetBodyParameter(parameters);
            var bodyParameterExpression = bodyParameter == null
                ? (Expression)Expression.Constant(null)
                : parameterExpressions
                    .Select(pe => pe.Item1)
                    .First(pe => pe.Name == bodyParameter.Name && pe.Type == bodyParameter.ParameterType);

            return GetExpressionForRequest(
                httpAttribute,
                returnType,
                parameters,
                parameterExpressions,
                bodyParameterExpression);
        }

        private LambdaExpression GetExpressionForRequest(
            HttpAttribute httpAttribute,
            Type returnType,
            ParameterInfo[] parameters,
            (ParameterExpression, ParameterInfo)[] parameterExpressions,
            Expression? bodyParameterExpression)
        {
            var parameterConstructor = typeof(UrlParameter).GetConstructors().First();
            var cancellationTokenParameter = GetCancellationTokenParameter(parameters);
            var cancellationTokenParameterExpression = cancellationTokenParameter == null
                ? (Expression)Expression.Default(typeof(CancellationToken))
                : parameterExpressions
                    .Select(pe => pe.Item1)
                    .First(pe => pe.Name == cancellationTokenParameter.Name && pe.Type == cancellationTokenParameter.ParameterType);
            var newParametersExpression = parameterExpressions
                .Where(p => p.Item2.GetCustomAttribute<BodyAttribute>() == null && p.Item2.ParameterType != typeof(CancellationToken))
                .Select(pe => Expression.New(parameterConstructor, Expression.Constant(pe.Item1.Name), pe.Item1.GetStringValueExpression()));
            var arrayInitExpression = Expression.NewArrayInit(typeof(UrlParameter), newParametersExpression);
            var callUrlBuildExpression = Expression.Call(
                Expression.Constant(_urlBuilder),
                _urlBuilder.GetBuildUrlMethodInfo(),
                Expression.Constant(httpAttribute.Url),
                arrayInitExpression);
            var urlBuilderParameterExpression = new List<Expression?> { callUrlBuildExpression, bodyParameterExpression, cancellationTokenParameterExpression };

            var callHttpClientMethodExpression = Expression.Call(
                Expression.Constant(_httpClient),
                GetMethodByRequestType(httpAttribute)
                    .MakeGenericMethod(returnType),
                urlBuilderParameterExpression.Where(pe => pe != null));
            var lambda = Expression.Lambda(callHttpClientMethodExpression, parameterExpressions.Select(pe => pe.Item1));

            return lambda;
        }

        private MethodInfo GetMethodByRequestType(HttpAttribute httpAttribute)
            => httpAttribute switch
            {
                PostAttribute => _httpClientPostMethod,
                GetAttribute => _httpClientGetMethod,
                PutAttribute => _httpClientPutMethod,
                DeleteAttribute => _httpClientDeleteMethod,
                PatchAttribute => _httpClientPatchMethod,
                _ => _httpClientGetMethod
            };
    }
}