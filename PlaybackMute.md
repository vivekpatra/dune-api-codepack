# playback\_mute #

## Summary ##

`playback_mute` indicates whether the playback is muted.

## Input ##

| **Parameter** | **Value(s)**         |
|:--------------|:---------------------|
| cmd           | set\_playback\_state |
| mute          | {value}              |
| [[timeout](timeout.md)] | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="playback_mute" value="{value}"/>
  ...
</command_result>
```

## Values ##

|           | **Mute** |
|:----------|:---------|
| **True**    | `1`      |
| **False**   | `0`      |
| **Default** | `0`      |