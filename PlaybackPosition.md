# playback\_position #
## Summary ##

`playback_position` indicates the playback position.

## Input ##

| **Parameter** | **Value(s)**         |
|:--------------|:---------------------|
| cmd           | set\_playback\_state |
| position      | {n}                  |
| [[timeout](timeout.md)] | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="playback_position" value="{n}"/>
  ...
</command_result>
```

## Values ##

The actual playback position in seconds.

## Notes ##

When changing this value: if the specified position is not a keyframe, the device will seek to the closest keyframe before that position.