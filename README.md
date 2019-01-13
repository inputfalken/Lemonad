<p align="center"><img src="https://s8.postimg.cc/mbxqv2gx1/LOGO_LEMONAD_README.jpg"></p>


[Logo](https://s8.postimg.cc/mbxqv2gx1/LOGO_LEMONAD_README.jpg) by [area55git](https://github.com/area55git) is licensed under [CC 4.0 International License.](https://creativecommons.org/licenses/by/4.0/)

## Status

[![Build status](https://lemonad-ci.visualstudio.com/Lemonad/_apis/build/status/Release?branchName=master)](https://lemonad-ci.visualstudio.com/Lemonad/_build/latest?definitionId=6)

### WIP (A lot may change)

[![NuGet](https://img.shields.io/nuget/v/Lemonad.ErrorHandling.svg)](https://www.nuget.org/packages/Lemonad.ErrorHandling/)
[![Build Status](https://lemonad-ci.vsrm.visualstudio.com/_apis/public/Release/badge/99c63ac5-7b1f-436d-b755-fbfa106c1853/2/2)](https://lemonad-ci.vsrm.visualstudio.com/_apis/public/Release/badge/99c63ac5-7b1f-436d-b755-fbfa106c1853/2/2)


## Summary

A functional **C#** library with data structures and functions whose main goal
is to have an [declarative](https://en.wikipedia.org/wiki/Declarative_programming)
approach to problem solving by using various
[HOF's](https://en.wikipedia.org/wiki/Higher-order_function#C#)
to achieve cleaner code bases & code which is easier to think about.

For example the `IResult<T, TError>` type,
which enables you to skip using exceptions for error handling.
The way `IResult<T, TError>` works is that there's
always one value present. Either it's the
value to the **left** or the value to the **right** (`TError`).
In this library, the value to the Left is considered **successfull**
and the right value is considered to be a **failure**.

## Example program 

```csharp
internal static class Program {
  private static int Main(string[] args) {
      return Validate(Console.ReadLine())
          .Match((string x) => {
              // The successful validation case
              Console.WriteLine($"Approved input '{x}'");
              return 0;
          }, (ExitCode x) => {
              // The failed validation case
              const string message = "Error: ";
              switch (x) {
                  case ExitCode.NullOrEmpty:
                      Console.WriteLine(message + "Input cannot be null or empty string.");
                      break;
                  case ExitCode.IsUpperCase:
                      Console.WriteLine(message + "Input cannot contain upper cased letters.");
                      break;
                  case ExitCode.Symbol:
                      Console.WriteLine(message + "Input cannot contain symbols.");
                      break;
                  default:
                      throw new ArgumentOutOfRangeException(nameof(x), x, null);
              }

              return (int) x;
          });
  }

  private static IResult<string, ExitCode> Validate(string input) {
      return Result.Value<string, ExitCode>(input)
          .Filter(string.IsNullOrWhiteSpace, (string _) => ExitCode.NullOrEmpty)
          .Map((string x) => x.Trim())
          .IsErrorWhen((string s) => s.Any(char.IsUpper), s => ExitCode.IsUpperCase)
          .IsErrorWhen((string x) => x.Any(char.IsSymbol), s => ExitCode.Symbol);
  }

  private enum ExitCode {
      NullOrEmpty = 1,
      IsUpperCase = 2,
      Symbol = 3
  }
}
```

Or if you do not care about handling a **failure** type,
you could use the `IMaybe<T>` type. The maybe type works
just like `IResult<T, TError>` except that there's
no **failure** (TError) type available.
It's works exactly as `Nullable<T>` (aka `T?`)
does except that you can use this with reference
types (`string`, `object`…) as well.

``` csharp
internal static class Program {
    private static int Main(string[] args) {
        return Validate(Console.ReadLine())
            .Match((string x) => {
                // The successful validation case
                Console.WriteLine($"Approved input '{x}'");
                return 0;
            }, () => {
                // The failed validation case
                Console.WriteLine("Error: Something went wrong");
                return 1;
            });
    }

    private static IMaybe<string> Validate(string input) {
        return Maybe.Value(input)
            .Filter(string.IsNullOrWhiteSpace)
            .Map((string x) => x.Trim())
            .IsNoneWhen((string s) => s.Any(char.IsUpper))
            .IsNoneWhen((string x) => x.Any(char.IsSymbol));
    }
}

```

## Current supported data types

* `IResult<out T, TError>`
* `IAsyncResult<out T, TError>`
* `IMaybe<out T>`

## To be added

* `IAsyncMaybe<out T>`

The out word means that they are covariant.
For more info please visit the article [Variance in Generic Interfaces (C#)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/covariance-contravariance/variance-in-generic-interfaces)
