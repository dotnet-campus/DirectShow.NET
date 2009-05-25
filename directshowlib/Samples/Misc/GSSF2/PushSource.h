/****************************************************************************
This sample is released as public domain.  It is distributed in the hope that
it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty
of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*****************************************************************************/

#pragma once

// Filter name strings
#define g_wszPushBitmap     L"Generic Sample Source Filter2"

/**********************************************
 *
 *  Interface Definitions
 *
 **********************************************/

DECLARE_INTERFACE_(IGenericSampleCB2, IUnknown) {
    STDMETHOD(CheckMediaType)(THIS_
            const AM_MEDIA_TYPE *amt
        ) PURE;
    STDMETHOD(OnMediaTypeChanged)(THIS_
            const AM_MEDIA_TYPE *amt
        ) PURE;
    STDMETHOD(SampleCallback)(THIS_
        IMediaSample *pSample
        ) PURE;

    STDMETHOD(OnThreadCreate)(THIS_
        ) PURE;

    STDMETHOD(OnThreadDestroy)(THIS_
        ) PURE;

    STDMETHOD(Startup)(THIS_
        DWORD *dwSeekingFlags,
        REFERENCE_TIME *rtDuration
        ) PURE;

    STDMETHOD(ChangeStart)(THIS_
        REFERENCE_TIME rtStart
        ) PURE;

    STDMETHOD(ChangeStop)(THIS_
        REFERENCE_TIME rtStop
        ) PURE;

    STDMETHOD(GetCurrentPosition)(THIS_
        REFERENCE_TIME *rtStop
        ) PURE;
};

DECLARE_INTERFACE_(IGenericSampleConfig, IUnknown) {

    STDMETHOD(SetPinMediaType) (THIS_
                const AM_MEDIA_TYPE *amt
             ) PURE;

    STDMETHOD(SetBitmapCB) (THIS_
                IGenericSampleCB2 *pfn
             ) PURE;
};

/**********************************************
 *
 *  Class declarations
 *
 **********************************************/

class AM_NOVTABLE CGenericSampleConfig :
    public IGenericSampleConfig,
    public CUnknown
{

public:
    DECLARE_IUNKNOWN

    CGenericSampleConfig(__in_opt LPCTSTR pName, __in_opt LPUNKNOWN pUnk) :

    CUnknown(pName, pUnk) {}
};

class CPushPinBitmap : public CSourceStream, public CSourceSeeking, public CGenericSampleConfig
{
protected:

    CMediaType m_amt;
    IGenericSampleCB2 *m_Callback;

public:

    CPushPinBitmap(HRESULT *phr, CSource *pFilter);
    ~CPushPinBitmap();

    // Override the version that offers exactly one media type
    HRESULT GetMediaType(CMediaType *pMediaType);
    HRESULT DecideBufferSize(IMemAllocator *pAlloc, ALLOCATOR_PROPERTIES *pRequest);
    HRESULT FillBuffer(IMediaSample *pSample);

    // Quality control
    // Not implemented because we aren't going in real time.
    STDMETHODIMP Notify(IBaseFilter *pSelf, Quality q)
    {
        return E_FAIL;
    }

    STDMETHODIMP NonDelegatingQueryInterface(REFIID riid, void ** ppv);

    STDMETHODIMP SetPinMediaType(const AM_MEDIA_TYPE *amt);
    STDMETHODIMP SetBitmapCB(IGenericSampleCB2 *pfn);

    // Override baseclass methods
    HRESULT CheckMediaType(const CMediaType *pmt);
    HRESULT CompleteConnect(IPin *pReceivePin);
    HRESULT OnThreadCreate(void);
    HRESULT OnThreadDestroy(void);

    STDMETHODIMP GetCurrentPosition(__out LONGLONG *pCurrent);

    // ======================================================
    // IGenericSampleConfig methods

    // Stop, flush, and restart pin threads.  Called by the COM object during IMediaSeekign
    STDMETHODIMP UpdateFromSeek(void);
    HRESULT         ChangeStart();
    HRESULT         ChangeStop();
    HRESULT         ChangeRate();
};


class CPushSourceBitmap : public CSource
{

private:
    // Constructor is private because you have to use CreateInstance
    CPushSourceBitmap(IUnknown *pUnk, HRESULT *phr);
    ~CPushSourceBitmap();

    CPushPinBitmap *m_pPin;

public:
    static CUnknown * WINAPI CreateInstance(IUnknown *pUnk, HRESULT *phr);
};

