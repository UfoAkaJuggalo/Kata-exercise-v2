Feature: Following
	Abillity to follow the user and get feed from combined timelines of followed users

Scenario: Subscribe to user timeline
	Given the first user name is "Alice"
	And the second user name is "Charlie"
	And the first user writes messages on her timeline
	When The second user subscribes to the first user
	Then the second user can get messages from the first user timeline

Scenario: Subscribe to users and get their combined timelines
	Given the first user name is "Alice"
	And the second user name is "Charlie"
	And the third user name is "Bob"
	And The first user adds two messages
	And The third user adds two messages
	And The second user subscribes to the both users
	When the second user try to get his news feed
	Then he get list of combined messages from both users