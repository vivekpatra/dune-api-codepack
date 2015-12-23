# playback\_duration #
## Summary ##

`playback_duration` indicates the playback duration.

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="playback_duration" value="{n}"/>
  ...
</command_result>
```

## Values ##

If the playback is a continuous stream (such as internet radio), the value is always 0.
In all other cases, the value is the actual playback duration in seconds.