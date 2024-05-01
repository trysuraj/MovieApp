import React from 'react';
import TitleSearchComponent from './component/TitleSearchComponent';
import IDSearchComponent from './component/IDSearchComponent';

function App() {
  return (
    <div>
      <h1 style={{background: '#a0a0a0',textAlign:'center'}}>Movie Search</h1>
      {/* <h3>Search Movie By Title or imdbID</h3>
      <div style={{ 
      // background: 'red', 
      display: 'flex', 
      flexDirection: 'row', 
      justifyContent: 'center',
      padding:'50px'
    }}> */}
         <TitleSearchComponent />
      <IDSearchComponent />
      {/* </div> */}
     
    </div>
  );
}

export default App;
