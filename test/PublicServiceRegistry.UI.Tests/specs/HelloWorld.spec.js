import { mutations, state } from '@/store';

// destructure assign `mutations`
const { LOADING_ON } = mutations;

describe('mutations', () => {
  it('INCREMENT', () => {
    LOADING_ON(state);
    expect(state.isLoading).toBe(true);
  });
});
