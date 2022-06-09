Feature: Reading
	Reading post from someone timeline

Scenario: Get post from someone timeline 
	Given The first user name is "Alice"
	And The second user name is "Bob"
	And the first user write messages on her timeline
	When the second user wants to see Alice timeline
	Then he gets all messages from the first user timeline