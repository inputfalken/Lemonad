using System;
using Lemonad.ErrorHandling;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp {
    public class Program {
        public static void Main(string[] args) {
            CreateWebHostBuilder(args).Build().Run();
            //Test.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }

    public class Test {
        public static void Run() {
            Console.WriteLine(
                JsonConvert.SerializeObject(ApiMethod(new PersonApiModel {FirstName = null, LastName = null})));
            Console.WriteLine(JsonConvert.SerializeObject(ApiMethod(new PersonApiModel {FirstName = "Foo", LastName = null})));
            Console.WriteLine(JsonConvert.SerializeObject(ApiMethod(new PersonApiModel {FirstName = "Foo", LastName = "Bar"})));
            Console.WriteLine(JsonConvert.SerializeObject(ApiMethod(new PersonApiModel {FirstName = "John", LastName = "Doe"})));
        }

        public static IActionResult ApiMethod(PersonApiModel person) {
            Either<IActionResult, PersonApiModel> rightWhen = Validate(person)
                .RightWhen((PersonApiModel x) => AppService(Map(x)), (ErrorModel x) => new BadRequestObjectResult(x));
            Either<IActionResult, SuccessModel> flatMapRight = Validate(person)
                .FlatMapRight((PersonApiModel x) => AppService(Map(x)), (ErrorModel x) => new BadRequestObjectResult(x));

            rightWhen.Match((IActionResult x) => (x), (PersonApiModel x) => new OkObjectResult(x));
            return flatMapRight.Match((IActionResult x) => (x), (SuccessModel x) => new OkObjectResult(x));
        }

        public static Either<IActionResult, PersonApiModel> Validate(PersonApiModel person) {
            return Either.Right<IActionResult, PersonApiModel>(person)
                .LeftWhen<IActionResult>(r => string.IsNullOrEmpty(r.FirstName),
                    () => new BadRequestObjectResult("FirstName is required."))
                .LeftWhen<IActionResult>(r => string.IsNullOrEmpty(r.LastName),
                    () => new BadRequestObjectResult("LastName is required."));
        }

        public static Either<ErrorModel, SuccessModel> AppService(PersonModel person) {
            if (person.LastName == "Bar")
                return new SuccessModel {Count = 4711};

            return new ErrorModel {Message = "Expected a 'Bar'"};
        }

        private static IActionResult Map(ErrorModel errorModel) {
            return new BadRequestObjectResult(errorModel.Message);
        }

        private static PersonModel Map(PersonApiModel person) {
            return new PersonModel {FirstName = person.FirstName, LastName = person.LastName};
        }

        private static IActionResult Map(SuccessModel successModel) {
            return new OkObjectResult(successModel);
        }
    }
}