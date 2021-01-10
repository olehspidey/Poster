namespace Poster.Core.Expressions
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
    using Exceptions;
    using Extensions;
    using Http.Clients;
    using Http.Clients.Abstract;
    using Http.Models;
    using Http.Serializers.Abstract;
    using Http.Url;
    using Http.Url.Abstract;
    using Types;

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
        public HttpClientExpressionBuilder(IHttpClientFactory httpClientFactory, IContentSerializer contentSerializer)
        {
            _httpClient = new PosterHttpClient(httpClientFactory, contentSerializer, nameof(IPoster));
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
                serviceMethod.GetCustomAttributes());

        /// <inheritdoc/>
        public LambdaExpression GetExpressionForNonGenericTask(MethodInfo serviceMethod)
            => GetExpressionForTask(
                serviceMethod.GetParameters(),
                typeof(Empty),
                serviceMethod.GetCustomAttributes());

        private static ParameterInfo? GetBodyParameter(IEnumerable<ParameterInfo> parameterInfos)
            => parameterInfos.FirstOrDefault(p => p.GetCustomAttribute<BodyAttribute>() != null);

        private static ParameterInfo? GetCancellationTokenParameter(IEnumerable<ParameterInfo> parameterInfos)
            => parameterInfos.FirstOrDefault(p => p.ParameterType == typeof(CancellationToken));

        private LambdaExpression GetExpressionForTask(ParameterInfo[] parameters, Type taskGenericArgumentType, IEnumerable<Attribute> customAttributes)
        {
            foreach (var attribute in customAttributes)
            {
                if (attribute is HttpAttribute httpAttribute)
                {
                    if(httpAttribute is PostAttribute || httpAttribute is PutAttribute || httpAttribute is PatchAttribute)
                        return GetExpressionForBodyRequest(
                            httpAttribute,
                            taskGenericArgumentType,
                            parameters);

                    return GetExpressionForBodilessRequest(
                        httpAttribute,
                        taskGenericArgumentType,
                        parameters);
                }
            }

            throw new ServiceInitializeException("Service methods should contain http attribute");
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
            var parameterConstructor = typeof(Parameter).GetConstructors().First();
            var cancellationTokenParameter = GetCancellationTokenParameter(parameters);
            var cancellationTokenParameterExpression = cancellationTokenParameter == null
                ? (Expression)Expression.Default(typeof(CancellationToken))
                : parameterExpressions
                    .Select(pe => pe.Item1)
                    .First(pe => pe.Name == cancellationTokenParameter.Name && pe.Type == cancellationTokenParameter.ParameterType);
            var newParametersExpression = parameterExpressions
                .Where(p => p.Item2.GetCustomAttribute<BodyAttribute>() == null && p.Item2.ParameterType != typeof(CancellationToken))
                .Select(pe => Expression.New(parameterConstructor, Expression.Constant(pe.Item1.Name), pe.Item1.GetStringValueExpression()));
            var arrayInitExpression = Expression.NewArrayInit(typeof(Parameter), newParametersExpression);
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