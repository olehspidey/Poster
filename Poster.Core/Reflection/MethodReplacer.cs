namespace Poster.Core.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;
    using Attributes;
    using Exceptions;
    using Expressions;
    using Expressions.Abstract;
    using Extensions;
    using Http.Clients;
    using Http.Clients.Abstract;
    using Http.Models;
    using Http.Serializers.Abstract;
    using Http.Url;
    using Http.Url.Abstract;
    using Moq;

    public class MethodReplacer
    {
        private readonly IHttpClient _httpClient;
        private readonly MethodInfo _httpClientGetMethod;
        private readonly IUrlBuilder _urlBuilder;
        private readonly IMockExpressionBuilder _mockExpressionBuilder;

        public MethodReplacer(IHttpClientFactory httpClientFactory, IContentSerializer contentSerializer)
        {
            _httpClient = new PosterHttpClient(httpClientFactory, contentSerializer);
            _mockExpressionBuilder = new MockExpressionBuilder();
            
            var httpClientType = _httpClient.GetType();
            
            _httpClientGetMethod = httpClientType.GetMethod(nameof(IHttpClient.GetAsync)) ?? throw new Exception("");
            _urlBuilder = new UrlBuilder();
        }
        
        public void ReplaceMethodsBodies<T>(IReadOnlyCollection<MethodInfo> methods, Mock<T> mock) where T : class
        {
            foreach (var method in methods)
            {
                if(method.DeclaringType == null)
                    continue;

                var methodParameters = method.GetParameters();
                var setupExpression = _mockExpressionBuilder.GetSetupExpression(method, methodParameters);
                var setup = mock.Setup(setupExpression, method.ReturnType);
                var setupType = setup.GetType();
                var methodInfo = setupType
                    .GetMethods()
                    .FirstOrDefault(m =>
                    {
                        if (m.Name != "Returns")
                            return false;
                        
                        var firstParameter = m.GetParameters().FirstOrDefault();

                        if (firstParameter == null)
                            return false;

                        var genericParams = firstParameter.ParameterType.GenericTypeArguments;
                        
                        return genericParams.Length == methodParameters.Length + 1 && genericParams.Last() == method.ReturnType;
                    });

                if (methodInfo == null)
                    throw new Exception(); //todo: need add exception

                methodInfo = methodInfo.MakeGenericMethod(methodParameters.Select(p => p.ParameterType).ToArray());

                if (method.ReturnType.BaseType == typeof(Task)) // todo: add normal check
                {
                    var expression = GetExpressionForGenericTask(method);
                    
                    methodInfo.Invoke(setup, new[] {expression.Compile()});
                }
            }
        }

        private LambdaExpression GetExpressionForGenericTask(MethodInfo methodInfo)
        {
            var customAttributes = methodInfo.GetCustomAttributes();

            foreach (var attribute in customAttributes)
            {
                if (attribute is GetAttribute getAttribute)
                {
                    return GetExpressionForGet(
                        getAttribute,
                        methodInfo.ReturnType.GenericTypeArguments.First(),
                        methodInfo.GetParameters());
                }
            }

            throw new ServiceInitializeException("Service methods should contain http attribute");
        }

        private LambdaExpression GetExpressionForGet(GetAttribute getAttribute, Type returnType, ParameterInfo[] parameters)
        {
            var parameterConstructor = typeof(Parameter).GetConstructors().First();
            var parameterExpressions = parameters
                .Select(p => Expression.Parameter(p.ParameterType, p.Name))
                .ToArray();
            var newParametersExpression = parameterExpressions
                .Select(pe => Expression.New(parameterConstructor, Expression.Constant(pe.Name), pe.GetStringValueExpression()));
            var arrayInitExpression = Expression.NewArrayInit(typeof(Parameter), newParametersExpression);
            var callUrlBuildExpression = Expression.Call(
                Expression.Constant(_urlBuilder),
                _urlBuilder.GetBuildUrlMethodInfo(),
                Expression.Constant(getAttribute.Url),
                arrayInitExpression);
            var callExpression = Expression.Call(
                Expression.Constant(_httpClient),
                _httpClientGetMethod
                    .MakeGenericMethod(returnType),
                callUrlBuildExpression);
            var lambda = Expression.Lambda(callExpression, parameterExpressions);

            return lambda;
        }
    }
}