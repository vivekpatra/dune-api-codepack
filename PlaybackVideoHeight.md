# playback\_video\_height #
## Summary ##

`playback_video_height` indicates the current video stream's height.

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="playback_video_height" value="{n}"/>
  ...
</command_result>
```

## Values ##

Actual value depends on the video stream.

|           | **Height**       |
|:----------|:-----------------|
| **Minimum** | `0`              |
| **Maximum** | `{osd_height}`   |