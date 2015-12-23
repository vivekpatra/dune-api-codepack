# last\_playback\_event #
## Summary ##

`last_playback_event` indicates the last playback event.

## Output ##

```
<?xml version="1.0" ?>
<command_result>
  ...
  <param name="last_playback_event" value="{value}"/>
  ...
</command_result>
```

## Values ##

| **Playback Event**             |
|:-------------------------------|
| media\_description\_changed    |
| no\_event                      |
| media\_read\_stalled           |
| end\_of\_media                 |
| external\_action               |
| media\_format\_not\_supported   |
| media\_open\_failed            |
| media\_read\_failed            |
| media\_protocol\_not\_supported |
| media\_permission\_denied      |
| internal\_error                |
| playlist\_changed              |
| media\_changed                 |
| audio\_stream\_changed         |
| subtitle\_stream\_changed      |
| pcr\_discontinuity             |