# video\_height #
## Summary ##

`video_height` indicates the playback window rectangle's height.

## Input ##

| **Parameter**      | **Value(s)**         |
|:-------------------|:---------------------|
| cmd                | set\_playback\_state |
| video\_fullscreen  | 0                    |
| video\_x           | {n}                  |
| video\_y           | {n}                  |
| video\_width       | {n}                  |
| video\_height      | {n}                  |
| [[timeout](timeout.md)]      | {n}                  |

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="video_height" value="{n}"/>
  ...
</command_result>
```

## Values ##

|           | **Height**                                   |
|:----------|:---------------------------------------------|
| **Minimum** | `0`                                          |
| **Maximum** | `{video_total_display_height} - {video_y}`   |
| **Default** | `{video_total_display_height}`               |

## Notes ##

This parameter has been renamed to [playback\_window\_rect\_height](PlaybackWindowRectHeight.md) starting with version 3.