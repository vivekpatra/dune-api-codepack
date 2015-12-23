# black\_screen #
## Summary ##

`black_screen` indicates whether to show a black screen during playback.


## Input ##

| **Parameter**  | **Value(s)**         |
|:---------------|:---------------------|
| cmd            | set\_playback\_state |
| black\_screen  | {value}              |
| [[timeout](timeout.md)]  | {n}                  |

## Output ##

It is impossible to determine the current state of the `black_screen` setting, unless you use a variable to remember the last value.

One way to do this is to assume that the last entered value is the current value. You can then monitor the `playback_state` parameter for playback events that would reset `black_screen` to its default value.

This method is ineffective because the actual value can be changed outside of your control.

## Values ##

|           | **Black Screen** |
|:----------|:-----------------|
| **True**    | `1`              |
| **False**   | `0`              |
| **Default** | `0`              |