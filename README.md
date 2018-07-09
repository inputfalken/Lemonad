<p align="center"><img src="https://s8.postimg.cc/mbxqv2gx1/LOGO_LEMONAD_README.jpg"></p>

## Status
[![Build status](https://ci.appveyor.com/api/projects/status/y57renjt90pk5huy/branch/master?svg=true)](https://ci.appveyor.com/project/inputfalken/lemonad/branch/master)
[![Build Status](https://travis-ci.org/inputfalken/Lemonad.svg?branch=master)](https://travis-ci.org/inputfalken/Lemonad)
[![Build Status](https://lemonad-ci.visualstudio.com/_apis/public/build/definitions/99c63ac5-7b1f-436d-b755-fbfa106c1853/2/badge)](https://lemonad-ci.visualstudio.com/Lemonad/_build/index?definitionId=2)

### WIP (A lot may change)
[![NuGet](https://img.shields.io/nuget/v/Lemonad.ErrorHandling.svg)](https://www.nuget.org/packages/Lemonad.ErrorHandling/)


## Summary

A functional **C#** library with data structures and functions whose main goal
is to have an [declarative](https://en.wikipedia.org/wiki/Declarative_programming)
approach to problem solving by using various
[HOF's](https://en.wikipedia.org/wiki/Higher-order_function#C#)
to achieve cleaner code bases & code which is easier to think about.

For example the `Result<T, TError>` type,
which enables you to skip using exceptions for error handling.
The way `Result<T, TError>` works is that there's
always one value present. Either it's the
value to the **left** or the value to the **right** (`TError`).
In this library, the value to the Left is considered **successfull**
and the right value is considered to be a **failure**.

The following example illustrates how you can perform a division
and return a string with an error message rather
than throwing an exception using a similiar message.

```csharp

Result<int, string> Divide(int x, int y) {
    if (y != 0)
        return  x / y;
    else
        return "Cannot divide with zero";
}


List<Result<int, string>> eithers = new List<Result<int, string>> {
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
List<int> successFulDivisions = eithers.Values().ToList();
IEnumerable<string> failedDivisions = eithers.Errors();

// Prints all the numbers where the 'y' parameter from the function is not 0.
// 2
// 1
// 5
foreach (int division in successFulDivisions) { Console.WriteLine(division); }

// Prints all the messages where the 'y' parameter from the function is 0.
// Cannot divide with zero
// Cannot divide with zero
// …
foreach (int message in failedDivisions) { Console.WriteLine(message); }


```

Or if you do not care about handling a **failure** type,
you could use the `Maybe<T>` type. The maybe type works
just like `Result<T, TError>` except that there's
no **failure** (TError) type available.
It's works exactly as `Nullable<T>` (aka `T?`)
does except that you can use this with reference
types (`string`, `object`…) as well.

The following example illustrates how you can perform a division
and instead of throwing an exception; you could return `Maybe<T>.None`

``` csharp
Maybe<int> Divide(int x, int y) {
    if (y != 0)
        return x / y;
    else {
        return Maybe<int>.None;
    }
}

List<Maybe<int>> maybes = new List<Maybe<int>> {
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

* ErrorHandling
  * `Result<TLeft, TRight>`
  * `Maybe<T>`

## To be added

* Async extensions methods for
  * `Result<TLeft, TRight>`
  * `Maybe<T>`

