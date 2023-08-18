import React, { useState } from 'react';

function NewVersion() {
    const [version, setVersion] = useState(0);
    const [release, setRelease] = useState(0);
    const [review, setReview] = useState(0);

    const handleSubmit = (event: { preventDefault: () => void; }) => {
        event.preventDefault();
        console.log(setVersion);
        console.log(setRelease);
        console.log(setReview);
    };

    return (
        <form onSubmit={handleSubmit} >
            <label>
                Version:
                <input type="number" value={version} onChange={e => setVersion(parseInt(e.target.value))} />
            </label>
            < br />
            <label>
                Release:
                <input type="number" value={release} onChange={e => setRelease(parseInt(e.target.value))} />
            </label>
            < br />
            <label>
                Review:
                <input type="number" value={review} onChange={e => setReview(parseInt(e.target.value))} />
            </label>
            < br />
            <button type="submit" > Submit </button>
        </form>
    );
}

export default NewVersion;