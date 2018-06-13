using System;
using System.Collections.Generic;
using Lemonad.ErrorHandling;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApp.ApiModels;
using WebApp.Models;

namespace WebApp.Controller {
    [Route("people")]
    public class PersonController : Microsoft.AspNetCore.Mvc.Controller {
        [HttpPost, Route("eitherSummarized")]
        public IActionResult EitherValidationSummarized([FromBody] PersonPostApiModel model) {
            return ApiValidation(model)
                .MapRight(x => new PersonModel {FirstName = x.FirstName, LastName = x.LastName})
                .Flatten(LastNameAppService, x => new PersonPostApiError {Message = x.Message, Model = model})
                .FlatMapRight(FirstNameAppService, x => new PersonPostApiError {Message = x.Message, Model = model})
                .Match<IActionResult>(BadRequest, Ok);
        }

        private static Either<PersonPostApiError, PersonPostApiModel> ApiValidation(PersonPostApiModel model) {
            var either = model.ToEitherRight<PersonPostApiError, PersonPostApiModel>();
            var apiValidation = new List<Either<PersonPostApiError, PersonPostApiModel>> {
                either.RightWhen(x => x.Age > 10, () => new PersonPostApiError {Message = "Age needs to be more than 10", Model = model}),
                either.Flatten(x => ValidateName(x.FirstName), s => new PersonPostApiError {Message = s, Model = model}),
                either.Flatten(x => ValidateName(x.LastName), s => new PersonPostApiError {Message = s, Model = model})
            };

            var errors = apiValidation
                .EitherLefts()
                .Select(x => x.Message)
                .ToArray();
            return errors.Any()
                ? (Either<PersonPostApiError, PersonPostApiModel>) new PersonPostApiError {Errors = errors, Message = "Invalid Api Validation.", Model = model}
                : model;
        }

        public static Either<ErrorModel, SuccessModel> LastNameAppService(PersonModel person) {
            return person.LastName == "Bar"
                ? (Either<ErrorModel, SuccessModel>) new SuccessModel {Count = 4711}
                : new ErrorModel {Message = "Expected a 'Bar'"};
        }

        public static Either<ErrorModel, SuccessModel> FirstNameAppService(PersonModel person) {
            return person.FirstName == "Foo"
                ? (Either<ErrorModel, SuccessModel>) new SuccessModel {Count = 4711}
                : new ErrorModel {Message = "Expected a 'Foo'"};
        }

        private static Either<string, string> ValidateName(string name) {
            return name.ToEitherRight<string, string>()
                .LeftWhen(string.IsNullOrWhiteSpace, () => "Name cannot be empty.")
                .RightWhen(s => s.All(char.IsLetter), () => "Name can only contain letters.")
                .RightWhen(s => char.IsUpper(s[0]), () => "Name must start with capital letter.");
        }
    }
}
