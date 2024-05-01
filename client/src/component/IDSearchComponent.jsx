import React, { useState } from 'react';

function IDSearchComponent() {
  const [searchID, setSearchID] = useState('');
const [searchResult, setSearchResult] = useState(null);
const [error, setError] = useState(null);
const [selectedResult, setSelectedResult] = useState(null);

  const handleIDSearch = () => {
    if (searchID) {
      fetch(`http://localhost:5087/api/Movie/${searchID}`)
        .then(response => response.json())
        .then(data => {
            console.log(data)
            setSearchResult(data);
            setSelectedResult(null); 
            setError(null);
          })
          .catch(error => {
            console.error('Error fetching data:', error);
            setSearchResult(null);
            setError('Error fetching data. Please try again.');
          });
    }
  };

  const handleResultClick = (result) => {
    setSelectedResult(prevSelectedResult => 
      prevSelectedResult === result ? null : result
    );
  };

  return (
    <div style={{ 
      background: '#f0f0f0', 
      minHeight: '100vh', 
      padding: '20px', 
      display: 'flex', 
      flexDirection: 'column', 
      alignItems: 'center' 
    }}>
      {/* <h1 style={{ marginBottom: '20px' }}>Movie Search</h1> */}
      <div style={{ marginBottom: '20px' }}>
        <input
          type="text"
          placeholder="Search by title"
          value={searchID}
          onChange={e => setSearchID(e.target.value)}
          style={{ padding: '10px', marginRight: '10px' }}
        />
        <button onClick={handleIDSearch} style={{ padding: '10px', backgroundColor: '#007bff', color: '#fff', border: 'none', borderRadius: '5px' }}>Search</button>
      </div>
      {error && <p style={{ color: 'red' }}>{error}</p>}
      {searchResult && (
        <div>
          <div 
            key={searchResult.title} 
            style={{
              display: selectedResult === searchResult ? 'none' : 'block',
              background: '#fff',
              border: '1px solid #ccc',
              borderRadius: '5px',
              padding: '20px',
              margin: '10px',
              boxShadow: '0 4px 8px 0 rgba(0,0,0,0.2)',
              cursor: 'pointer',
              width: '300px',
              textAlign: 'center'
            }}
            onClick={() => handleResultClick(searchResult)}
          >
            <h2>{searchResult.title}</h2>
            {searchResult.poster && <img src={searchResult.poster} alt="Movie Poster" style={{ maxWidth: '100%', height: 'auto', marginBottom: '10px', borderRadius:'10px' }} />}
            <p>Year: {searchResult.year}</p>
            <p>IMDb Rating: {searchResult.imdbID}</p>
            <p>Director: {searchResult.director}</p>
          </div>
        </div>
      )}
      {selectedResult && (
        <div style={{ marginTop: '20px', background: '#fff', padding: '20px', borderRadius: '5px', boxShadow: '0 4px 8px 0 rgba(0,0,0,0.2)', width: '300px', textAlign: 'center' }}   onClick={() => handleResultClick(!searchResult)}>
          <h2>{selectedResult.title}</h2>
          {selectedResult.poster && <img src={selectedResult.poster} alt="Movie Poster" style={{ maxWidth: '100%', height: 'auto', marginBottom: '10px' }} />}
          <p>Year: {selectedResult.year}</p>
          <p>IMDb Rating: {selectedResult.imdbID}</p>
          <p>Director: {selectedResult.director}</p>
          <p>Plot: {selectedResult.plot}</p>
          <p>Genre: {selectedResult.genre}</p>
          <p>Actors: {selectedResult.actors}</p>
        </div>
      )}
    </div>
  );
}

export default IDSearchComponent;
