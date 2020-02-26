<p align="center"><img src="https://s8.postimg.cc/mbxqv2gx1/LOGO_LEMONAD_README.jpg"></p>

[Logo](https://s8.postimg.cc/mbxqv2gx1/LOGO_LEMONAD_README.jpg) by [area55git](https://github.com/area55git) is licensed under [CC 4.0 International License.](https://creativecommons.org/licenses/by/4.0/)

[![Build status](https://lemonad-ci.visualstudio.com/Lemonad/_apis/build/status/Release?branchName=master)](https://lemonad-ci.visualstudio.com/Lemonad/_build/latest?definitionId=6)
[![NuGet](https://img.shields.io/nuget/v/Lemonad.ErrorHandling.svg)](https://www.nuget.org/packages/Lemonad.ErrorHandling/)

## Summary

A functional **C#** library with data structures and functions whose main goal
is to have an [declarative](https://en.wikipedia.org/wiki/Declarative_programming)
approach to problem solving by using various
[HOF's](https://en.wikipedia.org/wiki/Higher-order_function#C#)
to achieve cleaner code bases & code which is easier to think about.

## Data structures

### `IResult<out T, TError>`

One value will always be present. Either it's the
value to the **left** (`T`) or the value to the **right** (`TError`).
The value to the Left is considered **successfull**
and the right value is considered to be a **failure**.

There's also an asynchronous version of `IResult<out T, TError>` available called `IAsyncResult<T out, TError>`


For more information about how to use `IResult<out T, TError`, visit [this](https://fsharpforfunandprofit.com/rop/) article about **railway oriented programming**.

### `IMaybe<out T>`

Works like `IResult<T, TError>` but there's no `TError` available.
So you might have a value (`T`) present.

There's also an asynchronous version of `IMaybe<out T>` available called `IAsyncMaybe<T out>`


### Examples

Check the [samples](https://github.com/inputfalken/Lemonad/tree/master/samples)
