# OneBarker.NamecheapDns

## Purpose
This library is designed to make it very easy to work with Namecheap's DNS API.
In fact, this is an offshoot library from the one I started developing [OneBarker.NamecheapApi](https://github.com/barkerest/namecheap-api).
From the beginning, my only goal with that library was to manage DNS.
As I was developing that library, it became apparent that there was a better way to get what I wanted, and this library was born.

## Usage

Add a reference to this library and then create a DnsContext.
```c#
var config = new DnsContextConfig(DnsContextConfig.SandboxHost, "you", "your-key");
var context = new DnsContext(config, new NullLogger<DnsContext>());
```

The context will lazy load the TopLevelDomains and RegisteredDomains lists when they are first accessed.
The TopLevelDomains list is read-only and contains all the TLDs that Namecheap recognizes.
The RegisteredDomains list is also read-only, this library requires the domains to be registered externally.

This library was designed around managing the DNS host records.
Each registered domain comes with a HostRecords dictionary to do that, and each of the values in this dictionary
contains a record set accessor to simplify setting the various records.

```c#
// A records for www are 1.2.3.4 and 4.3.2.1 (the \n is used to separate multiple values when using this method).
context.HostRecords["www"].IPv4Addresses.SetValue("1.2.3.4\n4.3.2.1");

// AAAA record for www is fe80:12:34::56
context.HostRecords["www"].IPv6Addresses.SetValue("fe80:12:34::56");

// CNAME for w3 is www.example.com
context.HostRecords["w3"].CanonicalName.SetValue("www.example.com.");
```

TTL values can be set for all entries in a host record collection easily.
```c#
// A records for www need refreshed after 5 minutes.
context.HostRecords["www"].IPv4Addresses.ChangeTimeToLive(300);
```

You're not stuck with just changing the entire value either.
```c#
// Now the A records for www are 1.2.3.4, 4.3.2.1, and 4.3.3.4. And the 4.3.3.4 record inherited the 300 second TTL from before.
context.HostRecords["www"].IPv4Addresses.Add("4.3.3.4");
```

Removing values is easy as well.
```c#
// Remove the 4.3.2.1 A record from www. 
context.HostRecords["www"].IPv4Addresses.Remove("4.3.2.1");

// Remove all A records from www2.
context.HostRecords["www2"].IPv4Addresses.ClearValue();
```

## Design

Everything revolves around the IHostRecord interface.
There are 12 direct IHostRecord implementations to match to the 12 record types recognized by the Namecheap API.

- __HostAliasRecord__ (_ALIAS_)  
  The value must be a canonical name.  
  This value will be resolved by the Namecheap DNS servers to generate A/AAAA records by the same name.
- __HostCAAuthRecord__ (_CAA_)  
  The value must be formatted as '{FLAG} {TAG} "{CA}"'.  
  The FLAG should normally be 0.  
  The TAG will be 'issue', 'issuewild', or 'iodef'.  
  The CA value will be the certificate authority value for the TAG.
- __HostCNameRecord__ (_CNAME_)  
  The value must be a canonical name.
- __HostIPv4Record__ (_A_)  
  The most common record.  
  The value must be a valid IPv4 address.
- __HostIPv6Record__ (_AAAA_)  
  The value must be a valid IPv6 address.
- __HostMailEasyRecord__ (_MXE_)  
  The value must be a valid IPv4 address.  
  If the domain is setup for MailEasy, this record tells Namecheap where to relay your email.
- __HostMailRecord__ (_MX_)  
  The value must be a canonical name.  
  If the domain is setup for custom mail, this record defines the mailserver and preference.  
  The preference is automatically computed internally in the collection to keep the mail servers in the same order as entry.
- __HostMaskedRedirectRecord__ (_FRAME_)  
  The value must be a valid HTTP or HTTPS URL.  
  Visitors will open a frame hosted by Namecheap that then loads this URL in a hidden fashion.  
  The address bar of the browser will show the host name, but the masking is superficial since any robot or savvy user will be able to determine the actual source.
- __HostNameserverRecord__ (_NS_)  
  The value must be a canonical name.
- __HostPermanentRedirectRecord__ (_URL301_)  
  The value must be a valid HTTP or HTTPS URL.  
  Visitors will be redirected to the URL with a 301 redirect (moved permanently).
- __HostTextRecord__ (_TXT_)  
  The value can be anything up to 255 characters in length.
- __HostUnmaskedRedirectRecord__ (_URL_)  
  The value must be a valid HTTP or HTTPS URL.  
  Visitors will be redirected to the URL with a 302 redirect (moved temporarily).  

All of the direct implementations are read-only.  
Two additional implementations exist for easily managing these read-only objects.

- __HostRecordAccessor__  
  This special record wraps around a single IHostRecord object with the
  _IHostRecordAccessor_ interface.
  The interface provides the ability to set or clear the value, and also the
  ability to change the TTL.
- __HostRecordCollection__  
  This special record wraps around multiple IHostRecord objects.
  This interface inherits from _IHostRecordAccessor_ as well as _ICollection&lt;string&gt;_.
  As shown above, the SetValue method is designed to split a string on NL characters
  to set multiple values.  
  All records within the collection have the same name, type, and TTL.

These two implementations are then thrown together into a record set.
- __HostRecordSet__  
  Provides endpoints for each record type using the appropriate wrapper.  
  For instance, CanonicalName uses HostRecordAccessor and IPv4Addresses uses HostRecordCollection.  
  All records within the record set will have the same name.

And finally, the record sets are managed within a dictionary.
- __HostRecordDictionary__  
  A dictionary that creates record sets as needed to manage host records.  
  The ContainsKey method will return true if there are any host records with the specified name,
  however, the indexer will always return a HostRecordSet even if the name currently is not in use.

Host names are case-insensitive and will get converted to lowercase internally.
Values are case-sensitive.



## MX records

MX records are the only record type with a preference.  The preference from Namecheap
is the value previously set for the host record.  The preference sent to Namecheap
is calculated by the HostRecordCollection internally using a preference step.

The default preference step is 5.  So the first MX record gets 5, the second 10, the third
15, and so on.  If a multiple of 5 is no good, you can change the preference step using the
ChangePreference method on the MailServers collection.

The preference is automatically updated as records are added and removed and the entries
in the MailServers collection will remain in the same order as entry because of that.

## CAA records

The HostCAAuthRecord class provides helpers for accessing the Flag, Tag, and CertificateAuthority
values.  The HostRecordCollection does not expose these properties, but since these are not
likely to be values that get changed regularly I haven't bothered to implement an interface to
expose them.  Should this become a problem, I may be persuaded to come up with a workable interface.

## Basic Rules

- A named host with a CanonicalName record cannot have any other records.
- A named host cannot have redirect records with IP address or alias records.
- A named host can only have one of the redirects defined.
- A named host cannot have both MX and MXE records defined.
- A host name cannot exceed 63 characters in length (eg - "hello" in "hello.world.com").
- A domain name cannot exceed 70 characters (Namcheap limitation).
- A canonical name cannot exceed 255 characters.
- A canonical name should end with a period.

## License

Copyright (C) 2022 Beau Barker

Released under the [MIT License](https://opensource.org/licenses/MIT)

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

