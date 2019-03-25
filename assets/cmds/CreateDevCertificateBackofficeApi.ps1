# setup certificate properties including the commonName (DNSName) property for Chrome 58+
$certificate = New-SelfSignedCertificate `
    -Subject api.dienstverlening-test.basisregisters.vlaanderen `
    -DnsName api.dienstverlening-test.basisregisters.vlaanderen `
    -KeyAlgorithm RSA `
    -KeyLength 2048 `
    -NotBefore (Get-Date) `
    -NotAfter (Get-Date).AddYears(2) `
    -CertStoreLocation "cert:CurrentUser\My" `
    -FriendlyName "Dns specific Certificate for dienstverleningen .NET Core" `
    -HashAlgorithm SHA256 `
    -KeyUsage DigitalSignature, KeyEncipherment, DataEncipherment `
    -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.1")
$certificatePath = 'Cert:\CurrentUser\My\' + ($certificate.ThumbPrint)

# create temporary certificate path
$tmpPath = "C:\tmp"
If(!(test-path $tmpPath))
{
New-Item -ItemType Directory -Force -Path $tmpPath
}

# set certificate password here
$pfxPassword = ConvertTo-SecureString -String "dienstverlening!" -Force -AsPlainText
$pfxFilePath = "c:\tmp\api.dienstverlening-test.basisregisters.vlaanderen.pfx"
$cerFilePath = "c:\tmp\api.dienstverlening-test.basisregisters.vlaanderen.cer"

# create pfx certificate
Export-PfxCertificate -Cert $certificatePath -FilePath $pfxFilePath -Password $pfxPassword
Export-Certificate -Cert $certificatePath -FilePath $cerFilePath

# copy the certificate & pfx file to the app
# install the certificate by double clicking  or use the following script

<#
    # asumes the file are still located in $tmpPath

    # import the pfx certificate
    #Import-PfxCertificate -FilePath $pfxFilePath Cert:\LocalMachine\My -Password $pfxPassword -Exportable

    # trust the certificate by importing the pfx certificate into your trusted root
    #Import-Certificate -FilePath $cerFilePath -CertStoreLocation Cert:\CurrentUser\Root
#>