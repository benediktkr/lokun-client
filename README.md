lokun-client
=======

This is the Windows client for Lokun, a wrapper over the official OpenVPN client. It has tools and options specific to the Lokun infrastructure. 

![Lokun](media/logo.png)

We believe in free software and thus `lokun-client` is licensed under the GPL. 

Notes on privacy
===

In order to verify that the client is tunneling traffic through Lokun, this client hsa the feature to check https://lokun.is/connected.json. This is essentially the client phoning home and could allow for fingerprinting of users (although the person in the middle would also be capable of noticing the VPN connection itself). Users might need to be made aware of this. 

Maintainers 
====

Main: Benedikt Kristinsson <benedikt@lokun.is>

Karl Emil Karlsson <kalli@lokun.is>
