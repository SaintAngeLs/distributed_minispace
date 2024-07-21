import os
import requests

GITHUB_API_URL = "https://api.github.com"
GITLAB_API_URL = "https://gitlab.com/api/v4"

GITHUB_TOKEN = os.getenv('GITHUB_TOKEN')
GITLAB_TOKEN = os.getenv('GITLAB_TOKEN')
GITHUB_REPO = os.getenv('GITHUB_REPO')
GITLAB_REPO_1 = os.getenv('GITLAB_REPO_1')
GITLAB_REPO_2 = os.getenv('GITLAB_REPO_2')

def get_github_issues():
    url = f"{GITHUB_API_URL}/repos/{GITHUB_REPO}/issues"
    headers = {"Authorization": f"token {GITHUB_TOKEN}"}
    response = requests.get(url, headers=headers)
    response.raise_for_status()
    return response.json()

def get_github_pull_requests():
    url = f"{GITHUB_API_URL}/repos/{GITHUB_REPO}/pulls"
    headers = {"Authorization": f"token {GITHUB_TOKEN}"}
    response = requests.get(url, headers=headers)
    response.raise_for_status()
    return response.json()

def create_gitlab_issue(issue, gitlab_repo):
    url = f"{GITLAB_API_URL}/projects/{gitlab_repo.replace('/', '%2F')}/issues"
    headers = {"PRIVATE-TOKEN": GITLAB_TOKEN}
    data = {
        "title": issue['title'],
        "description": issue['body'] or '',
        "labels": ','.join(issue.get('labels', []))
    }
    response = requests.post(url, headers=headers, json=data)
    response.raise_for_status()
    return response.json()

def create_gitlab_merge_request(pr, gitlab_repo):
    url = f"{GITLAB_API_URL}/projects/{gitlab_repo.replace('/', '%2F')}/merge_requests"
    headers = {"PRIVATE-TOKEN": GITLAB_TOKEN}
    data = {
        "title": pr['title'],
        "description": pr['body'] or '',
        "source_branch": pr['head']['ref'],
        "target_branch": pr['base']['ref']
    }
    response = requests.post(url, headers=headers, json=data)
    response.raise_for_status()
    return response.json()

def sync_issues():
    issues = get_github_issues()
    for issue in issues:
        if 'pull_request' not in issue:
            create_gitlab_issue(issue, GITLAB_REPO_1)
            create_gitlab_issue(issue, GITLAB_REPO_2)

def sync_pull_requests():
    prs = get_github_pull_requests()
    for pr in prs:
        create_gitlab_merge_request(pr, GITLAB_REPO_1)
        create_gitlab_merge_request(pr, GITLAB_REPO_2)

if __name__ == "__main__":
    sync_issues()
    sync_pull_requests()
