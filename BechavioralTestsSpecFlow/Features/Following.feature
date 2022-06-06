Feature: Following
	Abillity to follow the user and get feed from combined timelines of followed users

Scenario: Subscribe to user timeline
	Given the first user named Alice
	And Alice wrote message on her timeline
	And the second user named Charlie
	When the second user subscribes to the first user
	Then he can get messages from the first user timeline

Scenario: Subscribe to users and get their combined timelines
	Given the first user named Alice
	And the second user named Charlie
	And the third user named Bob
	And the first user adds two messages
	And the third user adds two messages
	And the second user subscribes to the both users
	When the second users try to get his news feed
	Then he get list of combined messages from his subscriptions