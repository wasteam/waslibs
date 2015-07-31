using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test
{
    public static class ExceptionsAssert
    {
        public static T Throws<T>(Action testCode) where T : Exception
        {
            return (T)Throws(typeof(T), Execute(testCode));
        }

        public static async Task<T> ThrowsAsync<T>(Func<Task> testCode)
            where T : Exception
        {
            return (T)Throws(typeof(T), await ExecuteAsync(testCode));
        }

        static Exception Throws(Type exceptionType, Exception exception)
        {
            Assert.IsNotNull(exceptionType);

            if (exception == null)
                throw new AssertFailedException($"Null exception found, expected exception of type {exceptionType.Name}");

            if (!exceptionType.Equals(exception.GetType()))
                throw new AssertFailedException($"Exception found of type {exception.GetType().Name} , expected exception of type {exceptionType.Name}");

            return exception;
        }

        static Exception Execute(Action testCode)
        {
            Assert.IsNotNull(testCode);

            try
            {
                testCode();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        static async Task<Exception> ExecuteAsync(Func<Task> testCode)
        {
            Assert.IsNotNull(testCode);

            try
            {
                await testCode();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}