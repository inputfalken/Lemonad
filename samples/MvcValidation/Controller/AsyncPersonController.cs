using System.Linq;
using System.Threading.Tasks;
using Lemonad.ErrorHandling;
using Lemonad.ErrorHandling.Extensions;
using Microsoft.AspNetCore.Mvc;
using MvcValidation.ApiModels;
using MvcValidation.Models;

namespace MvcValidation.Controller {
    public class AsyncPersonController : Microsoft.AspNetCore.Mvc.Controller {
        [HttpPost]
        [Route("eitherSummarized")]
        public async Task<IActionResult> PostPerson([FromBody] PersonPostApiModel model) {
            return (await ApiValidation(model)
                    .Map(x => new PersonModel {FirstName = x.FirstName, LastName = x.LastName})
                    .Flatten(LastNameAppService, x => new PersonPostApiError {Message = x.Message, Model = model})
                    .FlatMap(FirstNameAppService, x => new PersonPostApiError {Message = x.Message, Model = model})
                    .ConfigureAwait(false)
                )
                .Match<IActionResult>(Ok, BadRequest);
        }

        private static Result<PersonPostApiModel, PersonPostApiError> ApiValidation(PersonPostApiModel model) {
            var apiValidation = model
                .ToResult<PersonPostApiModel, PersonPostApiError>()
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

        private static async Task<Result<SuccessModel, ErrorModel>> LastNameAppService(PersonModel person) {
            await Task.Delay(50);
            return person.LastName == "Bar"
                ? (Result<SuccessModel, ErrorModel>) new SuccessModel {Count = 4711}
                : new ErrorModel {Message = "Expected a 'Bar'"};
        }

        private static async Task<Result<SuccessModel, ErrorModel>> FirstNameAppService(PersonModel person) {
            await Task.Delay(50);
            return person.FirstName == "Foo"
                ? (Result<SuccessModel, ErrorModel>) new SuccessModel {Count = 4711}
                : new ErrorModel {Message = "Expected a 'Foo'"};
        }

        private static Result<string, string> ValidateName(string name) {
            return name.ToResult<string, string>()
                .IsErrorWhen(string.IsNullOrWhiteSpace, () => "Name cannot be empty.")
                .Filter(s => s.All(char.IsLetter), () => "Name can only contain letters.")
                .Filter(s => char.IsUpper(s[0]), () => "Name must start with capital letter.");
        }
    }
}