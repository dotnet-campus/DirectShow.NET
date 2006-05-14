/****************************************************************************
This sample is released as public domain.  It is distributed in the hope that 
it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty 
of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
*****************************************************************************/

#pragma once

// Filter name strings
#define g_wszPullSource     L"Generic Sample Pull Filter"


/**********************************************
 *
 *  Interface Definitions
 *
 **********************************************/

DECLARE_INTERFACE_(IGenericPullConfig, IUnknown) {

    STDMETHOD(SetMediaType) (THIS_
                AM_MEDIA_TYPE *amt
             ) PURE;

    STDMETHOD(SetEmbedded) (THIS_
                REFIID riid
             ) PURE;
};

/**********************************************
 *
 *  Class declarations
 *
 **********************************************/

class CPullOutputPin : public CBasePin,
    public IGenericPullConfig
{
protected:

    CMediaType m_amt;
    IUnknown *m_obj;

public:

    CPullOutputPin(HRESULT *phr, CSource *pFilter, CCritSec *cStateLock);
    ~CPullOutputPin();

    // Override the version that offers exactly one media type
    HRESULT GetMediaType(CMediaType *pMediaType);

    // See if the requested media type is the supported media type
    HRESULT CheckMediaType(const CMediaType *pMediaType)
    {
        HRESULT hr;
        CMediaType mt;

        CAutoLock lock(m_pLock);

        hr = GetMediaType(&mt);
        if (SUCCEEDED(hr))
        {
            if (mt == *pMediaType) 
            {
                hr = S_OK;
            }
            else
            {
                hr = E_FAIL;
            }
        }

        return hr;
    }

    // Retrieve the supported media types
    HRESULT GetMediaType(int iPosition, CMediaType *pMediaType)
    {
        HRESULT hr = S_OK;
        CAutoLock lock(m_pLock);

        if (iPosition == 0)
        {
            hr = GetMediaType(pMediaType);
        }
        else if (iPosition > 0)
        {
            pMediaType = NULL;
            hr = VFW_S_NO_MORE_ITEMS;
        }
        else
        {
            pMediaType = NULL;
            hr = E_INVALIDARG;
        }

        return hr;
    }

    HRESULT STDMETHODCALLTYPE
    BeginFlush()
    {
        return S_OK;
    }

    HRESULT STDMETHODCALLTYPE
    EndFlush()
    {
        return S_OK;
    }

    // ----------------------------------------------------------------------
    // override this to reveal our property interface
    STDMETHODIMP NonDelegatingQueryInterface(REFIID riid, void ** ppv);
    DECLARE_IUNKNOWN;

    STDMETHODIMP SetMediaType(AM_MEDIA_TYPE *amt);
    STDMETHODIMP SetEmbedded(REFIID riid);
};


class CPullSourceFilter : public CSource
{

private:
    // Constructor is private because you have to use CreateInstance
    CPullSourceFilter(IUnknown *pUnk, HRESULT *phr);
    ~CPullSourceFilter();

    CPullOutputPin *m_pPin;

public:
    static CUnknown * WINAPI CreateInstance(IUnknown *pUnk, HRESULT *phr);  

    // Override (most of) the base class's implementation of pin handling.
    // We can't use CSourceStream pins.

    HRESULT     AddPin(CSourceStream *) { return E_NOTIMPL; }
    HRESULT     RemovePin(CSourceStream *) { return E_NOTIMPL; }

    int FindPinNumber(IPin *iPin)  
    {  
        int iRet;

        if (iPin == m_pPin)
        {
            iRet = 0;
        }
        else
        {
            iRet = -1;
        }
    }

    int GetPinCount()
    {
        return 1;
    }

    CBasePin * GetPin(int n)
    {
        if (n == 0)
        {
            return m_pPin;
        }
        else
        {
            return NULL;
        }
    }

};
