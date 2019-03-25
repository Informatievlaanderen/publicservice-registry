#!/bin/bash
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null && pwd )"

# less specific domains go first
$DIR/hostsUtil.sh addhost dienstverlening-test.basisregisters.vlaanderen
$DIR/hostsUtil.sh addhost api.dienstverlening-test.basisregisters.vlaanderen
