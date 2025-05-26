ParPal \
Made by: Ray Hettleman, Daniel Ahn, Jackson Sanders \
Instructions: 
1) Run GolfScoreCard.sln in JetBrains Rider 
2) In your browser, access http://localhost:500 

Design Decisions & Architecture: \
The top layer of ParPals is the UI. Using Razor or .cshtml we were able to \
make several front-end pages that allow the user to interact with the \
application. \
The middle layer is where calculations occur. Like differentials and \
handicaps. Additionally, HTTP requests are handled on this level. ParPal \
fetches golf course data from GolfCourseAPI.com.\
The bottom layer uses SQL databases to store account data, previous \
round data, and the course slope and rating for each course that is \
played.

Singleton:
We implemented the singleton design pattern in the HandicapCalculator class
since it is responsible for calculating handicap differentials and averages.
We made it a sealed class to ensure no other classes can inherit from it. 
We then used this calculator is ScoresController to perform handicap calculations. 
This allows our logic to be centralized and avoid duplicate code.

Observer:
The observer pattern is also implemented within the handicap calculations. When the
users handicap changes, they are notified so that they stay as up to date as 
possible regarding their handicap.

Strategy: \
The strategy pattern is used in the handicap calculations. In normal, \
USGA handicap calculations, there is a .96 "excellency" multiplier, which \
is multiplied by the normal calculations to get an accurate handicap. The \
other strategy does not take into account this excellency multiplier. Both \
are shown in the users profile.
