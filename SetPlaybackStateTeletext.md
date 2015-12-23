# set\_playback\_state: Teletext #
## Summary ##

The following parameters provide an interface with teletext, available when using a TV tuner.

## Syntax ##

| **Parameter**                                 | **Value(s)**         | **Example**                | **API version(s)** |
|:----------------------------------------------|:---------------------|:---------------------------|:-------------------|
| [cmd](Cmd.md)                                   | set\_playback\_state | cmd=set\_playback\_state   | ≥ 1                |
| [[teletext\_enabled](TeletextEnabled.md)]        | 0 | 1                | teletext\_enabled=1        | ?                  |
| [[teletext\_mix\_mode](TeletextMixMode.md)]       | 0 | 1                | teletext\_mix\_mode=1      | ?                  |
| [[teletext\_page\_number](TeletextPageNumber.md)] | 100 .. 899           | teletext\_page\_number=100 | ?                  |
| [[teletext\_key](TeletextKey.md)]                | {code}               | teletext\_key=KEY\_0       | ?                  |
| [[timeout](Timeout.md)]                         | {n}                  | timeout=20                 | ≥ 1                |

### Examples ###

Enable teletext:

`http://dune/cgi-bin/do?cmd=set_playback_state&teletext_enabled=1`


---


Enable teletext mix mode:

`http://dune/cgi-bin/do?cmd=set_playback_state&teletext_mix_mode=1`


---


Go to teletext page 888:

`http://dune/cgi-bin/do?cmd=set_playback_state&teletext_page_number=888`


---


Send the down key to teletext:

`http://dune/cgi-bin/do?cmd=set_playback_state&teletext_key=KEY_DOWN`