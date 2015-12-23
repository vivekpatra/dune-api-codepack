# Introduction #

These are the bugs that are so far discovered in the API and need to be taken care of by Dune HD developers.

Bugs that are fixed in an officially released firmware version are removed from this page, time is too limited to keep things backwards compatible.

Also don't forget to check out the [feature requests](FeatureRequests.md) page.

# Bugs #

**command results parameter: player\_state**

Problem: _player\_state_ indicates _file\_playback_ when the player is actually in _navigator_ mode after playback has ended.

Status: Broken in version 1-3

| |Expectation|Result|
|:|:----------|:-----|
|right after boot|navigator  |navigator|
|during playback|file\_playback|file\_playback|
|after playback ended|navigator  | **file\_playback**|


---

**launch\_media\_url: media\_url=storage\_name://optical\_drive/**

Problem: more often than not, launch\_media\_url fails to launch the inserted disc.

Status: Broken in version 3


---

**command results parameter: playback\_url**

Problem: during DVD or Blu-ray playback, the playback\_url value is always an empty string.

Status: Broken in version 3

---

**set\_playback\_state: speed=-128**

Status: Broken in version 1-3

| |Expectation|Result|
|:|:----------|:-----|
|Behaviour|rewind 1/2x|rewind 1/2x|
|OSD caption|<font size='3' face='webdings'>7</font> 1/2|<font size='3' face='webdings'>7</font> 2|
|Command result value|-128       |2147483520|
|16-bit binary|1111 1111 1111 1111 1111 1111 1000 0000|0111 1111 1111 1111 1111 1111 1000 0000|




---

**set\_playback\_state: speed=-64**

Status: Broken in version 1-3

| |Expectation|Result|
|:|:----------|:-----|
|Behaviour|rewind 1/4x|rewind 1/4x|
|OSD caption|<font size='3' face='webdings'>7</font> 1/4|<font size='3' face='webdings'>7</font> 4|
|Command result value|-64        |1073741760|
|16-bit binary|1111 1111 1111 1111 1111 1111 1100 0000|0011 1111 1111 1111 1111 1111 1100 0000|


---

**set\_playback\_state: speed=-32**

Status: Broken in version 1-3

| |Expectation|Result|
|:|:----------|:-----|
|Behaviour|rewind 1/8x|rewind 1/8x|
|OSD caption|<font size='3' face='webdings'>7</font> 1/8|<font size='3' face='webdings'>7</font> 8|
|Command result value|-32        |536870880|
|16-bit binary|1111 1111 1111 1111 1111 1111 1110 0000|0001 1111 1111 1111 1111 1111 1110 0000|


---

**set\_playback\_state: speed=-16**

Status: Broken in version 1-3

| |Expectation|Result|
|:|:----------|:-----|
|Behaviour|rewind 1/16x|rewind 1/16x|
|OSD caption|<font size='3' face='webdings'>7</font> 1/16|<font size='3' face='webdings'>7</font> 16|
|Command result value|-16        |268435440|
|16-bit binary|1111 1111 1111 1111 1111 1111 1111 0000|0000 1111 1111 1111 1111 1111 1111 0000|


---

**set\_playback\_state: speed=-8**

Status: Broken in version 3

| |Expectation|Result|
|:|:----------|:-----|
|Behaviour|rewind 1/32x|rewind 1/32x|
|OSD caption|<font size='3' face='webdings'>7</font> 1/32|???   |
|Command result value|-8         |134217720|
|16-bit binary|1111 1111 1111 1111 1111 1111 1111 1000|0000 0111 1111 1111 1111 1111 1111 1000|