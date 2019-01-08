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

The following example illustrates how you can perform a division
and return a string with an error message rather
than throwing an exception using a similiar message.

```csharp

private static IResult<int, string> Divide(int left, int right) {
    return left != 0
        ? Result.Value<int, string>(left / right)
        : Result.Error<int, string>($"Can not divide '{left}' with '{right}'.");
}


var results = new List<IResult<int, string>> {
  Divide(4, 2),
  Divide(3, 0),
  Divide(3, 3),
  Divide(4, 0),
  Divide(5, 0),
  Divide(6, 0),
  Divide(7, 0),
  Divide(8, 0),
  Divide(10, 2)
};
List<int> successFulDivisions = results.Values().ToList();
IEnumerable<string> failedDivisions = results.Errors();

// Prints all the numbers where the 'y' parameter from the function is not 0.
// 2
// 1
// 5
foreach (int division in successFulDivisions) { Console.WriteLine(division); }

// Prints all the messages where the parameter 'right' is equal to 0.
// Can not divide {left} with {right}
// Can not divide {left} with {right}
// …
foreach (int message in failedDivisions) { Console.WriteLine(message); }


```

Or if you do not care about handling a **failure** type,
you could use the `IMaybe<T>` type. The maybe type works
just like `IResult<T, TError>` except that there's
no **failure** (TError) type available.
It's works exactly as `Nullable<T>` (aka `T?`)
does except that you can use this with reference
types (`string`, `object`…) as well.

The following example illustrates how you can perform a division
and instead of throwing an exception; you could return `IMaybe<T>.None`

``` csharp
IMaybe<int> Divide(int x, int y) {
    if (y != 0)
        return Maybe.Value(x / y);
    else {
        return Maybe.None<int>();
    }
}

List<IMaybe<int>> maybes = new List<IMaybe<int>> {
    Divide(4, 2),
    Divide(3, 0),
    Divide(3, 3),
    Divide(4, 0),
    Divide(5, 0),
    Divide(6, 0),
    Divide(7, 0),
    Divide(8, 0),
    Divide(10, 2)
};

List<int> successFulDivisions = maybes.MaybeSome().ToList();

// Prints all the numbers where the 'y' parameter from the function is not 0.
// 2
// 1
// 5
foreach (int division in successFulDivisions) { Console.WriteLine(division); }


```

## Current supported data types

* `IResult<out T, TError>`
* `IAsyncResult<out T, TError>`
* `IMaybe<out T>`

## To be added

* `IAsyncMaybe<out T>`

The out word means that they are covariant.
For more info please visit the article [Variance in Generic Interfaces (C#)](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/covariance-contravariance/variance-in-generic-interfaces)
