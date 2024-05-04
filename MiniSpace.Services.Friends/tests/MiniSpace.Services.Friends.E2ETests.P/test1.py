import requests
import json
import pytest

class TestFriendsAPI:
    base_url = "http://localhost:5004"
    base_friends_url = "http://localhost:5012"
    token = ""

    def authenticate_and_get_token(self):
        """Authenticate and retrieve token for tests."""
        login_url = f"{self.base_url}/sign-in"
        login_payload = {
            "email": "friend1@email.com",
            "password": "friend1"
        }
        response = requests.post(login_url, json=login_payload)
        if response.status_code != 200:
            raise Exception("Authentication failed, cannot proceed with tests.")
        token_response = response.json()
        self.token = token_response['accessToken']

    def setup_method(self):
        """Setup that runs before every test method."""
        self.authenticate_and_get_token()

    def test_invite_friend_already_friends_error(self):
        """Test inviting a friend who is already a friend returns the correct error."""
        invite_url = f"{self.base_friends_url}/friends/b202a5fe-afbd-4894-95e3-41aff99f430c/invite"
        headers = {'Authorization': f'Bearer {self.token}'}
        payload = {
            "inviterId": "fd0176f4-736c-49cd-b57a-523d544ae3d3",
            "inviteeId": "b202a5fe-afbd-4894-95e3-41aff99f430c"
        }
        response = requests.post(invite_url, json=payload, headers=headers)
        assert response.status_code == 400
        assert "Already friends" in response.text
