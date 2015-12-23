# hide\_osd #
## Summary ##

`hide_osd` indicates whether to hide overlay graphics (volume indicator, position indicator, setting menus ...).


## Input ##

| **Parameter** | **Value(s)**         |
|:--------------|:---------------------|
| cmd           | set\_playback\_state |
| hide\_osd     | {value}              |
| [[timeout](timeout.md)] | {n}                  |

## Output ##

It is impossible to determine the current state of the `hide_osd` setting, unless you use a variable to remember the last value.

One way to do this is to assume that the last entered value is the current value. You can then monitor the `playback_state` parameter for playback events that would reset `hide_osd` to its default value.

This method is ineffective because the actual value can be changed outside of your control.

## Values ##

|           | **Hide OSD** |
|:----------|:-------------|
| **True**    | `1`          |
| **False**   | `0`          |
| **Default** | `0`          |