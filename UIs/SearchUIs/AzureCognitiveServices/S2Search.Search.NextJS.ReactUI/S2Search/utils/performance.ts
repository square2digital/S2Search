// Performance monitoring utilities for development and production

export const performanceUtils = {
  // Development-only function to measure render performance
  measureRender: (componentName: string, fn: () => void) => {
    if (process.env.NODE_ENV === 'development' && typeof window !== 'undefined') {
      const startTime = window.performance.now();
      fn();
      const endTime = window.performance.now();
      console.log(`${componentName} render time: ${endTime - startTime}ms`);
    } else {
      fn();
    }
  },

  // Measure async operations
  measureAsync: async <T>(operationName: string, fn: () => Promise<T>): Promise<T> => {
    if (process.env.NODE_ENV === 'development' && typeof window !== 'undefined') {
      const startTime = window.performance.now();
      const result = await fn();
      const endTime = window.performance.now();
      console.log(`${operationName} took: ${endTime - startTime}ms`);
      return result;
    } else {
      return await fn();
    }
  },

  // Log Web Vitals in development
  logWebVitals: (metric: any) => {
    if (process.env.NODE_ENV === 'development') {
      console.log('Web Vital:', {
        name: metric.name,
        value: metric.value,
        id: metric.id,
        delta: metric.delta,
      });
    }
  },

  // Log memory usage in development
  logMemory: () => {
    if (process.env.NODE_ENV === 'development' && typeof window !== 'undefined' && 'memory' in window.performance) {
      const memory = (window.performance as any).memory;
      console.log('Memory Usage:', {
        used: Math.round(memory.usedJSHeapSize / 1024 / 1024) + ' MB',
        total: Math.round(memory.totalJSHeapSize / 1024 / 1024) + ' MB',
        limit: Math.round(memory.jsHeapSizeLimit / 1024 / 1024) + ' MB',
      });
    }
  },
};

// Hook for measuring component performance
export const usePerformanceMonitor = (componentName: string) => {
  if (process.env.NODE_ENV === 'development' && typeof window !== 'undefined') {
    const startTime = window.performance.now();
    
    return () => {
      const endTime = window.performance.now();
      console.log(`${componentName} total lifecycle: ${endTime - startTime}ms`);
    };
  }
  
  return () => {}; // No-op in production or SSR
};

// Bundle size analyzer helper
export const logBundleInfo = () => {
  if (process.env.NODE_ENV === 'development' && typeof window !== 'undefined') {
    setTimeout(() => {
      const scripts = Array.from(document.scripts);
      const totalSize = scripts.reduce((acc, script) => {
        return acc + (script.src ? 1 : 0); // Simple count for development
      }, 0);
      console.log(`Total script tags loaded: ${totalSize}`);
    }, 1000);
  }
};
