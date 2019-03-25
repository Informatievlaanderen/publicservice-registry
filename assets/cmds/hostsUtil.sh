
#!/bin/bash

# copy from https://gist.github.com/irazasyed/a7b0a079e7727a4315b9

# PATH TO YOUR HOSTS FILE
# ETC_HOSTS=/etc/hosts
ETC_HOSTS=/c/Windows/System32/drivers/etc/hosts

# DEFAULT IP FOR HOSTNAME
IP="127.0.0.1"

# Hostname to add/remove.
HOSTNAME=$2

removehost() {
    echo "removing host";
    if [ -n "$(grep $HOSTNAME $ETC_HOSTS)" ]
    then
        echo "$HOSTNAME Found in your $ETC_HOSTS, Removing now...";
        sed -i".bak" "/$HOSTNAME/d" $ETC_HOSTS
    else
        echo "$HOSTNAME was not found in your $ETC_HOSTS";
    fi
}

addhost() {
    echo "adding host";
    HOSTS_LINE="$IP\t$HOSTNAME"
    if [ -n "$(grep $HOSTNAME $ETC_HOSTS)" ]
        then
            echo "$HOSTNAME already exists : $(grep $HOSTNAME $ETC_HOSTS)"
        else
            echo "Adding $HOSTNAME to your $ETC_HOSTS";
            sh -c -e "echo -e '$HOSTS_LINE' >> $ETC_HOSTS";

            if [ -n "$(grep $HOSTNAME $ETC_HOSTS)" ]
                then
                    echo "$HOSTNAME was added succesfully \n $(grep $HOSTNAME $ETC_HOSTS)";
                else
                    echo "Failed to Add $HOSTNAME, Try again!";
            fi
    fi
}

$@
