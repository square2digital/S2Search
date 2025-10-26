import React, { useState } from 'react';
import Chip from '@mui/material/Chip';
import Stack from '@mui/material/Stack';
import Button from '@mui/material/Button';

const TestFacets = () => {
  const [facetSelectors, setFacetSelectors] = useState([
    {
      facetKey: 'model',
      facetDisplayText: '2 Series',
      luceneExpression: "model eq '2 Series'",
      checked: true,
    },
    {
      facetKey: 'model',
      facetDisplayText: '300',
      luceneExpression: "model eq '300'",
      checked: true,
    },
    {
      facetKey: 'model',
      facetDisplayText: '208',
      luceneExpression: "model eq '208'",
      checked: true,
    },
  ]);

  const [searchTriggerCount, setSearchTriggerCount] = useState(0);

  const handleDelete = facetToDelete => () => {
    const updatedArray = facetSelectors.filter(
      facet => facet.facetDisplayText !== facetToDelete.facetDisplayText
    );

    setFacetSelectors(updatedArray);

    // Simulate search trigger
    setSearchTriggerCount(prev => prev + 1);
  };

  const resetFacets = () => {
    setFacetSelectors([
      {
        facetKey: 'model',
        facetDisplayText: '2 Series',
        luceneExpression: "model eq '2 Series'",
        checked: true,
      },
      {
        facetKey: 'model',
        facetDisplayText: '300',
        luceneExpression: "model eq '300'",
        checked: true,
      },
      {
        facetKey: 'model',
        facetDisplayText: '208',
        luceneExpression: "model eq '208'",
        checked: true,
      },
    ]);
    setSearchTriggerCount(0);
  };

  return (
    <div style={{ padding: '20px' }}>
      <h1>Facet Chip Deletion Test</h1>

      <div style={{ marginBottom: '20px' }}>
        <h3>Search Trigger Count: {searchTriggerCount}</h3>
      </div>

      <div style={{ marginBottom: '20px' }}>
        <h3>Current Facet Selectors ({facetSelectors.length}):</h3>
        <Stack direction="row" spacing={1}>
          {facetSelectors.map((facetChip, index) => (
            <Chip
              key={`${facetChip.facetKey}-${facetChip.facetDisplayText}-${index}`}
              label={facetChip.facetDisplayText}
              onDelete={handleDelete(facetChip)}
              color="primary"
              variant="outlined"
            />
          ))}
        </Stack>
      </div>

      <div style={{ marginBottom: '20px' }}>
        <Button variant="contained" onClick={resetFacets}>
          Reset Facets
        </Button>
      </div>

      <div>
        <h3>Instructions:</h3>
        <p>
          1. Click the X on any facet chip to delete it
          <br />
          2. Watch the console for debug logs
          <br />
          3. Notice how the Search Trigger Count increases
          <br />
          4. Observe that the chip is removed from the display
          <br />
          5. Use Reset Facets to restore all chips
        </p>
      </div>

      <div
        style={{ marginTop: '20px', border: '1px solid #ccc', padding: '10px' }}
      >
        <h4>Debug Info:</h4>
        <pre>{JSON.stringify(facetSelectors, null, 2)}</pre>
      </div>
    </div>
  );
};

export default TestFacets;
