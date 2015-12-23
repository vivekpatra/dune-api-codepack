# video\_width #
## Summary ##

`video_width` indicates the playback window rectangle's width.

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
  <param name="video_width" value="{n}"/>
  ...
</command_result>
```

## Values ##

|           | **Width**                                   |
|:----------|:--------------------------------------------|
| **Minimum** | `0`                                         |
| **Maximum** | `{video_total_display_width} - {video_x}`   |
| **Default** | `{video_total_display_width}`               |

## Notes ##

This parameter has been renamed to [playback\_window\_rect\_width](PlaybackWindowRectWidth.md) starting with version 3.