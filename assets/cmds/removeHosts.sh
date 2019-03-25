#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null && pwd )"

# more specific domains go first
$DIR/hostsUtil.sh removehost api.dienstverlening-test.basisregisters.vlaanderen
$DIR/hostsUtil.sh removehost dienstverlening-test.basisregisters.vlaanderen
