using System.Linq;
using System.Threading.Tasks;
using Lemonad.ErrorHandling;
using Microsoft.AspNetCore.Mvc;
using MvcValidation.ApiModels;
using MvcValidation.Models;

namespace MvcValidation.Controller {
    public class AsyncPersonController : Microsoft.AspNetCore.Mvc.Controller {
        private static Result<PersonPostApiModel, PersonPostApiError> ApiValidation(PersonPostApiModel model) {
            var apiValidation = model
                .ToResult(x => true, () => default(PersonPostApiError))
                .Multiple(
                    x => x.Filter(y => y.Age > 10,
                        () => new PersonPostApiError {Message = "Age needs to be more than 10", Model = model}),
                    x => x.Flatten(y => ValidateName(y.FirstName),
                        s => new PersonPostApiError {Message = s, Model = model}),
                    x => x.Flatten(y => ValidateName(y.LastName),
                        s => new PersonPostApiError {Message = s, Model = model})
                );

            return apiValidation.Match(x => x, x => {
                return (Result<PersonPostApiModel, PersonPostApiError>) new PersonPostApiError {
                    Errors = x.Select(y => y.Message).ToArray(),
                    Message = "Invalid Api Validation.",
                    Model = model
                };
            });
        }

        private static async Task<Result<SuccessModel, ErrorModel>> FirstNameAppService(PersonModel person) {
            await Task.Delay(50);
            return person.FirstName == "Foo"
                ? (Result<SuccessModel, ErrorModel>) new SuccessModel {Count = 4711}
                : new ErrorModel {Message = "Expected a 'Foo'"};
        }

        private static async Task<Result<SuccessModel, ErrorModel>> LastNameAppService(PersonModel person) {
            await Task.Delay(50);
            return person.LastName == "Bar"
                ? (Result<SuccessModel, ErrorModel>) new SuccessModel {Count = 4711}
                : new ErrorModel {Message = "Expected a 'Bar'"};
        }

        [HttpPost]
        [Route("eitherSummarized")]
        public Task<IActionResult> PostPerson([FromBody] PersonPostApiModel model) {
            return ApiValidation(model)
                // Using match inside this scope is currently too complex since it requires all type params to be supplied.
                .Map(x => new PersonModel {FirstName = x.FirstName, LastName = x.LastName})
                .ToAsyncResult()
                .Flatten(x => LastNameAppService(x).ToAsyncResult(),
                    x => new PersonPostApiError {Message = x.Message, Model = model})
                .FlatMap(x => FirstNameAppService(x).ToAsyncResult(),
                    x => new PersonPostApiError {Message = x.Message, Model = model})
                .Match<IActionResult>(Ok, BadRequest);
        }

        private static Result<string, string> ValidateName(string name) {
            return name
                .ToResult(x => string.IsNullOrWhiteSpace(x) == false, () => "Name cannot be empty.")
                .Filter(s => s.All(char.IsLetter), () => "Name can only contain letters.")
                .Filter(s => char.IsUpper(s[0]), () => "Name must start with capital letter.");
        }
    }
}