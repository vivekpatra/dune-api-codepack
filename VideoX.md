# video\_x #
## Summary ##

`video_x` indicates the playback window rectangle's left margin.

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
  <param name="video_x" value="{n}"/>
  ...
</command_result>
```

## Values ##

|           | **Left Margin**                                 |
|:----------|:------------------------------------------------|
| **Minimum** | `0`                                             |
| **Maximum** | `{video_total_display_width} - {video_width}`   |
| **Default** | `0`                                             |

## Notes ##

This parameter has been renamed to [playback\_window\_rect\_x](PlaybackWindowRectX.md) starting with version 3.