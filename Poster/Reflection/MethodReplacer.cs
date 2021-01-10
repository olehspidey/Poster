namespace Poster.Core.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;
    using Abstract;
    using Exceptions;
    using Expressions;
    using Expressions.Abstract;
    using Http.Serializers.Abstract;
    using Moq;
    using Types;

    /// <summary>
    /// Represents implementation of <see cref="IMethodReplacer"/> that works with <see cref="Mock"/> object.
    /// </summary>
    public class MethodReplacer : IMethodReplacer
    {
        private readonly IMockExpressionBuilder _mockExpressionBuilder;
        private readonly IHttpClientExpressionBuilder _httpClientExpressionBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodReplacer"/> class.
        /// </summary>
        /// <param name="httpClientFactory">Http client factory that will use for <see cref="HttpClient"/> building.</param>
        /// <param name="contentSerializer"><see cref="IContentSerializer"/> that will be used for http content serialization.</param>
        public MethodReplacer(IHttpClientFactory httpClientFactory, IContentSerializer contentSerializer)
        {
            _mockExpressionBuilder = new MockExpressionBuilder();
            _httpClientExpressionBuilder = new HttpClientExpressionBuilder(httpClientFactory, contentSerializer);
        }

        /// <inheritdoc/>
        public void ReplaceMethodsBodies<T>(IReadOnlyCollection<MethodInfo> methods, Mock<T> mock)
            where T : class
        {
            foreach (var method in methods)
            {
                var serviceMethodReturnTypeInfo = GetServiceMethodReturnTypeInfo(method);
                var serviceMethodReturnType = method.ReturnType;

                if (!serviceMethodReturnTypeInfo.IsTask)
                    throw new ServiceInitializeException("All methods should have Task or Task<T> return type");

                if (!serviceMethodReturnTypeInfo.IsGenericTask)
                    serviceMethodReturnType = typeof(Task<Empty>);

                var methodParameters = method.GetParameters();
                var setupExpression = _mockExpressionBuilder.GetSetupExpression(method, methodParameters);
                var setup = mock.Setup(setupExpression, serviceMethodReturnType);
                var setupType = setup.GetType();
                var mockReturnsMethod = setupType
                    .GetMethods()
                    .FirstOrDefault(m =>
                    {
                        if (m.Name != "Returns")
                            return false;

                        var firstParameter = m.GetParameters().FirstOrDefault();

                        if (firstParameter == null)
                            return false;

                        var genericParams = firstParameter.ParameterType.GenericTypeArguments;

                        return genericParams.Length == methodParameters.Length + 1 && genericParams.Last() == serviceMethodReturnType;
                    });

                if (mockReturnsMethod == null)
                    throw new Exception(); // todo: need add exception

                if(mockReturnsMethod.IsGenericMethodDefinition)
                    mockReturnsMethod = mockReturnsMethod.MakeGenericMethod(methodParameters.Select(p => p.ParameterType).ToArray());

                var mockReturnsBodyParametersExpression = serviceMethodReturnTypeInfo.IsGenericTask
                    ? _httpClientExpressionBuilder.GetExpressionForGenericTask(method)
                    : _httpClientExpressionBuilder.GetExpressionForNonGenericTask(method);

                mockReturnsMethod.Invoke(setup, new object[] { mockReturnsBodyParametersExpression.Compile() });
            }
        }

        private ServiceMethodReturnTypeInfo GetServiceMethodReturnTypeInfo(MethodInfo serviceMethod)
        {
            if (serviceMethod.ReturnType == typeof(Task))
                return new ServiceMethodReturnTypeInfo(true);

            if (serviceMethod.ReturnType.IsGenericType && serviceMethod.ReturnType == typeof(Task<>).MakeGenericType(serviceMethod.ReturnType.GetGenericArguments()))
                return new ServiceMethodReturnTypeInfo(true, true);

            return new ServiceMethodReturnTypeInfo(false);
        }

        private class ServiceMethodReturnTypeInfo
        {
            public ServiceMethodReturnTypeInfo(bool isTask, bool isGenericTask = false)
            {
                IsTask = isTask;
                IsGenericTask = isGenericTask;
            }

            public bool IsTask { get; }

            public bool IsGenericTask { get; }
        }
    }
}