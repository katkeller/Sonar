-> start
=== start ===
Friend: So I guess we have some time to catch up now.
You: I guess. Or we could focus on what we're--
Friend: So what have you been up to since high school? I heard you went to the University of Washington. That's pretty far from home, especially by Roosevelt standards. Were you running from something?
*   [I had to leave.] -> start.had_to_leave
*   [No, I just like Seattle.] -> start.like_seattle
    ->END
= had_to_leave
You: Yep, I had to get as far from home as possible.
-> END
= like_seattle
You: No, I wasn't running away. I just liked it there more than here... or anywhere else, I guess. Plus, they gave me a pretty good scholarship.
Friend: Ha, yeah. And we all know how much you needed that.
You: What?
Friend: I-- Shit, I'm really sorry. We all made a lot of jokes like that way back when, but that came off way bitcher than I remember.
*   [It's fine.] -> start.its_fine
*   [It's wasn't funny then either] -> start.wasnt_funny
-> END
= its_fine
You: It's fine, don't worry about it. We all did some dumb shit then, and it was a long time ago.
-> END
= wasnt_funny
You: Honestly, it wasn't really that funny then either.
Friend: Ha... yeah. Time does put a lot of stuff into perspective, huh? Like I remember this time...
-> END