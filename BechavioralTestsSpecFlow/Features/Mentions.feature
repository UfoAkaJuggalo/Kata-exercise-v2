Feature: Mentions

Ability to mention user in message


Scenario: Mention user in message
	Given a first user name is "Bob"
	And a second user name is "Charlie"
	When the first user mentions the second user in a message
	Then the second user is added to mentions in message
