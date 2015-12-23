# player\_state #
## Summary ##

`player_state` indicates the current player state.

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="player_state" value="{value}"/>
  ...
</command_result>
```

## Values ##

| **Value**           | **Summary**                                |
|:--------------------|:-------------------------------------------|
| standby             | Standby mode.                              |
| loading             | Transitioning between two player states.   |
| navigator           | Navigator mode.                            |
| torrent\_downloads  | Showing the torrent client.                |
| photo\_viewer       | Showing an image or image slideshow.       |
| black\_screen       | Showing a black screen.                    |
| dvd\_playback       | Playing a DVD structure.                   |
| bluray\_playback    | Playing a Blu-ray structure.               |
| file\_playback      | Playing an arbitrary file.                 |
| safe\_mode          | Changing display mode.                     |

## Notes ##

versions 1 through 3 have a very annoying bug where the player\_state remains _file\_playback_ after playback has ended.
I've reported this and received confirmation that this will be fixed, so let's keep our fingers crossed.