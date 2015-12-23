# video\_zoom #
## Summary ##

`video_zoom` indicates the video zoom mode.

## Input ##

| **Parameter**        | **Value(s)**         |
|:---------------------|:---------------------|
| cmd                  | set\_playback\_state |
| video\_zoom          | {value}              |
| [[timeout](timeout.md)]        | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="video_zoom" value="{value}"/>
  ...
</command_result>
```

## Values ##

|                                     | **Zoom**             | **API version(s)** |
|:------------------------------------|:---------------------|:-------------------|
| **Default**                           | `normal`             | ≥ 2                |
| **Full screen**                       | `full_enlarge`       | ≥ 3                |
| **Stretch to full screen**            | `full_stretch`       | ≥ 3                |
| **Non-linear stretch**                | `fill_screen`        | ≥ 2                |
| **Non-linear stretch to full screen** | `full_fill_screen`   | ≥ 2                |
| **Enlarge**                           | `enlarge`            | ≥ 2                |
| **Make wider**                        | `make_wider`         | ≥ 2                |
| **Make taller**                       | `make_taller`        | ≥ 2                |
| **Cut edges**                         | `cut_edges`          | ≥ 2                |
| **Custom**                            | `other`              | ≥ 2                |

## Notes ##

All of these values can be used in [set\_playback\_state](SetPlaybackStateCommand.md) commands, except _other_, which is the return value when custom zoom settings were applied using the zoom button on a remote control.