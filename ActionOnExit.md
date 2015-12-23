# action\_on\_exit #
## Summary ##

`action_on_exit` indicates which player state to return to when playback ends.

## Input ##

| **Parameter**      | **Value(s)**         |
|:-------------------|:---------------------|
| cmd                | set\_playback\_state |
| action\_on\_finish | {value}              |
| [[timeout](timeout.md)]      | {n}                  |

## Output ##

It is impossible to determine the current state of the `action_on_exit` setting, unless you use a variable to remember the last value.

One way to do this is to assume that the last entered value is the current value. You can then monitor the `playback_state` parameter for playback events that would reset `action_on_exit` to its default value.

This method is ineffective because the actual value can be changed outside of your control.

## Values ##

|                | **Action**       |
|:---------------|:-----------------|
| **Main Screen**  | `main_screen`    |
| **Black Screen** | `black_screen`   |

## Notes ##

This parameter currently doesn't work. It is recognized as valid, but attempting to change its value has no effect.