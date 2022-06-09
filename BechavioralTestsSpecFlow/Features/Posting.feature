Feature: Posting
	User can post messages to his own timeline

Scenario: User can publish messages to a personal timeline
	Given the user name is "Alice"
	And message with text "it's just a test"
	When user publish the message
	Then message should be added to Alice timeline