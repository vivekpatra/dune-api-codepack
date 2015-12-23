# action\_on\_finish #
## Summary ##

Indicates whether to repeat playback.

## Input ##

| **Parameter**      | **Value(s)**         |
|:-------------------|:---------------------|
| cmd                | set\_playback\_state |
| action\_on\_finish | {value}              |
| [[timeout](timeout.md)]      | {n}                  |

## Output ##

It is impossible to determine the current state of the `action_on_finish` setting, unless you use a variable to remember the last value.

One way to do this is to assume that the last entered value is the current value. You can then monitor the `playback_state` parameter for playback events that would reset `action_on_finish` to its default value.

This method is ineffective because the actual value can be changed outside of your control.

## Values ##

|                    | **Action On Finish** |
|:-------------------|:---------------------|
| **Restart Playback** | restart\_playback    |
| **Exit**             | exit                 |