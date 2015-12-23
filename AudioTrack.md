# audio\_track #
## Summary ##

`audio_track` indicates the active audio track.

## Input ##

| **Parameter** | **Value(s)**         |
|:--------------|:---------------------|
| cmd           | set\_playback\_state |
| audio\_track  | {n}                  |
| [[timeout](timeout.md)] | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="audio_track" value="{n}"/>
  ...
</command_result>
```

## Values ##


|            | **Track**                     |
|:-----------|:------------------------------|
| **Minimum**  | `0`                           |
| **Maximum**  | `{sum of audio tracks} - 1`   |
| **Default**  | `0*`                          |

`*`: the preferred language setting (if set) takes precedence over the default value.