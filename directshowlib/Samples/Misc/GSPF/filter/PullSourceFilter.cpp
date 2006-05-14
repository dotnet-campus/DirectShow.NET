/****************************************************************************
This sample is released as public domain.  It is distributed in the hope that 
it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty 
of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
*****************************************************************************/

// See the readme.txt file for a discussion of how to use this filter

#include <streams.h>

#include "PullSource.h"
#include "PullGuids.h"

/**********************************************
 *
 *  CPullOutputPin Class
 *
 *
 **********************************************/

CPullOutputPin::CPullOutputPin(HRESULT *phr, CSource *pFilter, CCritSec *cStateLock)
      : CBasePin(
            NAME("Pull output pin"),
            pFilter,
            cStateLock,
            phr,
            L"Output",
            PINDIR_OUTPUT),
            m_obj(NULL)
{
}

CPullOutputPin::~CPullOutputPin()
{
    if (m_obj != NULL)
    {
        m_obj->Release();
        m_obj = NULL;
    }
}


// GetMediaType: This method tells the downstream pin what types we support.

// Here is how CSourceStream deals with media types:
//
// If you support exactly one type, override GetMediaType(CMediaType*). It will then be
// called when (a) our filter proposes a media type, (b) the other filter proposes a
// type and we have to check that type.
//
// If you support > 1 type, override GetMediaType(int,CMediaType*) AND CheckMediaType.
//
// In this case we support only one type.

HRESULT CPullOutputPin::GetMediaType(CMediaType *pMediaType)
{
    HRESULT hr = S_OK;

    CheckPointer(pMediaType, E_POINTER);

    CAutoLock cAutoLock(m_pLock);

    // If the mediatype hasn't been set, just fail here.
    if (m_amt.IsValid())
    {
        hr = pMediaType->Set(m_amt);
    }
    else
    {
        hr = E_FAIL;
    }

    return hr;
}

// Standard COM stuff done with a baseclasses flavor

STDMETHODIMP CPullOutputPin::NonDelegatingQueryInterface(REFIID riid, void **ppv)
{
    HRESULT hr = E_FAIL;

    if (riid == IID_IGenericPullConfig)
    {
        hr = GetInterface((IGenericPullConfig *) this, ppv);
    }
    else
    {
        hr = CBasePin::NonDelegatingQueryInterface(riid, ppv);
    }

    // If all else fails, try the embedded object
    if (FAILED(hr) && (m_obj != NULL))
    {
        hr = m_obj->QueryInterface(riid, ppv);
    }

    return hr;
}


/**********************************************
 *
 *  IGenericPullConfig methods 
 *
 **********************************************/

STDMETHODIMP CPullOutputPin::SetMediaType(AM_MEDIA_TYPE *amt)
{
    HRESULT hr = S_OK;

    CheckPointer(amt, E_POINTER);
    CAutoLock cAutoLock(m_pLock);

    // Only allow one init
    if (!m_amt.IsValid())
    {
        // Don't allow GUID_NULL for the major type (since that's what I
        // use to indicate an unpopulated MediaType).
        if (!IsEqualGUID(amt->majortype, GUID_NULL))
        {
            hr = m_amt.Set(*amt);
        }
        else
        {
            hr = MAKE_HRESULT(1, FACILITY_WIN32, ERROR_INVALID_PARAMETER);
        }
    }
    else
    {
        hr = MAKE_HRESULT(1, FACILITY_WIN32, ERROR_ALREADY_INITIALIZED);
    }

    return hr;
}

// Set the embedded object

STDMETHODIMP CPullOutputPin::SetEmbedded(REFIID riid)
{
    CAutoLock cAutoLock(m_pLock);
    HRESULT hr;

    if (m_obj == NULL)
    {
        // Create an instance of the COM class, get the IUnknown.  Note that we MUST
        // use aggregation, otherwise you can't qi from the embedded object back to the IPin,
        // and there are filters that try to do exactly that.
        hr = CoCreateInstance(riid, GetOwner(), CLSCTX_INPROC_SERVER, IID_IUnknown, (LPVOID *)&m_obj);
    }
    else
    {
        hr = MAKE_HRESULT(1, FACILITY_WIN32, ERROR_ALREADY_INITIALIZED);
    }

    return hr;
}

/**********************************************
 *
 *  CPullSourceFilter Class
 *
 **********************************************/

CPullSourceFilter::CPullSourceFilter(IUnknown *pUnk, HRESULT *phr)
           : CSource(NAME("PullSourceFilter"), pUnk, CLSID_GenericSamplePullFilter)
{
    m_pPin = new CPullOutputPin(phr, this, &m_cStateLock);

    if (phr)
    {
        if (m_pPin == NULL)
        {
            *phr = E_OUTOFMEMORY;
        }
        else
        {
            *phr = S_OK;
        }
    }
}

CPullSourceFilter::~CPullSourceFilter()
{
    if (m_pPin)
    {
        delete m_pPin;
        m_pPin = NULL;
    }
}

CUnknown * WINAPI CPullSourceFilter::CreateInstance(IUnknown *pUnk, HRESULT *phr)
{
    CPullSourceFilter *pNewFilter = new CPullSourceFilter(pUnk, phr );

    if (phr)
    {
        if (pNewFilter == NULL)
        {
            *phr = E_OUTOFMEMORY;
        }
        else
        {
            *phr = S_OK;
        }
    }

    return pNewFilter;
}
